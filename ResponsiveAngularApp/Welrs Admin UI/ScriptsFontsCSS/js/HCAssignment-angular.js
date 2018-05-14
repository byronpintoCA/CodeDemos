var hcaApp = angular.module("hcaApp", []);

function getRootFolder() {
    var pathArray = location.pathname.split('/');
    var appPath = "/";
    for (var i = 1; i < pathArray.length - 1; i++) {
        appPath += pathArray[i] + "/";
    }
    return appPath;
}

var rootFolder = getRootFolder();

hcaApp.factory('hcaFactory', function ($http) {

    var factory = {};

    factory.loadAllData = function (token) {

        //alert(rootFolder + "HCAssignment/FailedQueItems");
        return $http({
            headers: { "__RequestVerificationToken": token },
            method: "GET",
            url: rootFolder + "HCAssignment/FailedQueItems",
            contentType: 'application/json'

        });

    };

    factory.getHl7Data = function (token, msh_id, dataCallback) {

        return $http({
            headers: { "__RequestVerificationToken": token },
            method: "GET",
            url: rootFolder + "HCAssignment/HL7Data?msh_id=" + msh_id,
            contentType: 'application/json'

        }).success(function (data) {
            var payloadData = {
                "msh_id": msh_id,
                "hl7data": data
            }
            dataCallback(payloadData);
        });

    };

    factory.SaveData = function (token, msh_id, note, changeSet) {
        //alert(note);

        var payLoad = { "MSH_ID": msh_id, "Note": note, "ChangeSet": changeSet };
        return $http({
            headers: { "__RequestVerificationToken": token },
            method: "POST",
            url: rootFolder + "HCAssignment/Save",
            dataType: 'JSON',
            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
            data: payLoad
        });
    }

    return factory;

});

var _hcaCartScope;
hcaApp.controller("hcaCartController", function (hcaFactory, $scope, $http) {

    _hcaCartScope = $scope;
    var cart = this;
    cart.Items = [];

    this.setCart = function (data) {
        cart.Items = data;
    }

});

var _hcaScope;

hcaApp.controller("hcaController", function (hcaFactory, $scope, $http) {

    _hcaScope = $scope;
    var vm = this;
    var daFactory = hcaFactory;
    vm.data = [];

    this.loadAllData = function (dataCallBack, errorCallback) {

        var token = $('input[name="__RequestVerificationToken"]').val();
        daFactory.loadAllData(token)
            .success(function (data) {
                dataCallBack(data);
            })
        .error(function (data, status, headers, config) {
            if (null !== errorCallback) {
                errorCallback(data, status, headers, config);
            }
        });
    };

    this.setVM = function (data, isCollapsed) {
        vm.data = data;
        vm.loadHL7Data(isCollapsed);
    }

    this.loadHL7Data = function (isCollapsed) {
        ///alert('called');
        if (vm.data.HL7 === null && isCollapsed === false) {
            vm.data.HL7 = "Loading .. Please wait .";

            var token = $('input[name="__RequestVerificationToken"]').val();

            daFactory.getHl7Data(token, vm.data.MSH_ID, function (payload) {
                if (vm.data.MSH_ID === payload.msh_id) {
                    vm.data.HL7 = payload.hl7data;
                }
                else {
                    // find the appropriate object and set its HL7
                }

            }).error(function (data, status, headers, config) {
                vm.data.HL7 = "Unable to load data : Response code : " + status;
            });
        }
    }

    this.AddHC = function (objectContext, healthCondition) {

        var result;
        var daObRIndex;
        switch (objectContext.NodeType) {
            case "1":
                result = this.UpdateAllItems(objectContext, healthCondition);
                break;
            case "2":
                daObRIndex = this.GetObrIndex(objectContext.OBR_ID);
                result = this.UpdateAll_OBR_Items(this.data.MSH_ID, daObRIndex, objectContext, healthCondition);
                break;
            case "3":
                var changeSET = []
                daObRIndex = this.GetObrIndex(objectContext.OBR_ID);
                var daObXIndex = this.GetObxIndex(daObRIndex, objectContext.OBX_ID);
                changeSET.push(this.Update_HC_Single_OBX(this.data.MSH_ID, this.data.ObrList[daObRIndex].OBR_ID,
                    this.data.ObrList[daObRIndex].ObxList[daObXIndex], objectContext, healthCondition));
                result = changeSET;
                break;

        }


        this.AddUpdateFromChangeSet(result);
        //alert('Added');
        this.CheckIfObrCancelIsNeeded();
        return result;
    }

    this.RemoveAllHC = function (context) {

        var changeSET = [];

        vm.data.ObrList.forEach(function (obr) {

            if (context != null) {
                if (context.OBR_ID !== obr.OBR_ID) return;
            }

            if (obr.ObxList === null || obr.ObxList.length === 0) {
                changeSET.push(vm.Remove_HC_Single_OBR(vm.data.MSH_ID, obr, null));
            }
            else {
                for (var i = 0; i < obr.ObxList.length; i++) {
                    changeSET.push(vm.Remove_HC_Single_OBX(vm.data.MSH_ID, obr.OBR_ID, obr.ObxList[i]));
                }
            }
        });

        this.RemoveItemsFromChangeSet(changeSET);
        //alert('Remove');
        this.CheckIfObrCancelIsNeeded();
        return changeSET;
    }

    this.RemoveHealth = function (objectContext) {

        var changeSET = [];
        var daObRIndex = this.GetObrIndex(objectContext.OBR_ID);

        if (this.data.ObrList[daObRIndex].ObxList === null || this.data.ObrList[daObRIndex].ObxList.length === 0) {
            changeSET.push(this.Remove_HC_Single_OBR(this.data.MSH_ID, this.data.ObrList[daObRIndex], objectContext));
        }
        else {
            var daObXIndex = this.GetObxIndex(daObRIndex, objectContext.OBX_ID);
            changeSET.push(this.Remove_HC_Single_OBX(this.data.MSH_ID, this.data.ObrList[daObRIndex].OBR_ID,
                this.data.ObrList[daObRIndex].ObxList[daObXIndex]));
        }

        this.RemoveItemsFromChangeSet(changeSET);
        //alert('Remove');
        this.CheckIfObrCancelIsNeeded();
        return changeSET;
    }

    this.CheckIfObrCancelIsNeeded = function () {

        vm.data.ObrList.forEach(function (obr) {

            if (null == obr.ObxList) {
                obr.ShowOBRCancelAll = false;
                return;
            }

            var hasValue = false;
            for (var i = 0; i < obr.ObxList.length; i++) {
                if (!(obr.ObxList[i].HCAssignment == null || obr.ObxList[i].HCAssignment.length <= 0)) {
                    hasValue = true;
                }
            }

            if (true === hasValue) {
                obr.ShowOBRCancelAll = true;
            }
            else {
                obr.ShowOBRCancelAll = false;
            }

        });
    }

    this.AddUpdateFromChangeSet = function (changes) {
        if (!vm.data.AllChanges) {
            vm.data.AllChanges = [];
            changes.forEach(function (change) {
                vm.data.AllChanges.push(change);
            });
        }
        else {

            for (var i = 0; i < changes.length; i++) {

                var updated = false;
                for (var j = 0; j < vm.data.AllChanges.length; j++) {
                    if (vm.data.AllChanges[j].NodeType === changes[i].NodeType &&
                    vm.data.AllChanges[j].MSH_ID === changes[i].MSH_ID &&
                    vm.data.AllChanges[j].OBR_ID === changes[i].OBR_ID &&
                    vm.data.AllChanges[j].OBX_ID === changes[i].OBX_ID) {
                        vm.data.AllChanges[j].HealthCondition = changes[i].HealthCondition;
                        updated = true;
                    }
                }

                if (updated === false) vm.data.AllChanges.push(changes[i]);
            }
        }
    }

    this.RemoveItemsFromChangeSet = function RemoveItemsFromChangeSet(changes) {

        if (vm.data.AllChanges == null) {
            return;
        }

        for (var i = 0; i < changes.length; i++) {
            for (var j = 0; j < vm.data.AllChanges.length; j++) {

                if (vm.data.AllChanges[j].NodeType === changes[i].NodeType &&
                    vm.data.AllChanges[j].MSH_ID === changes[i].MSH_ID &&
                    vm.data.AllChanges[j].OBR_ID === changes[i].OBR_ID &&
                    vm.data.AllChanges[j].OBX_ID === changes[i].OBX_ID) {
                    vm.data.AllChanges.splice(j, 1);

                }
            }
        }

        if (vm.data.AllChanges.length === 0) {
            vm.data.AllChanges = null;

        }
    }

    this.UpdateAllItems = function (objectContext, healthCondition) {
        var changeSET = [];
        var arrayLength = this.data.ObrList.length;
        for (var i = 0; i < arrayLength; i++) {
            var changes = this.UpdateAll_OBR_Items(this.data.MSH_ID, i, objectContext, healthCondition);
            var changeLength = changes.length;
            for (var j = 0; j < changeLength; j++) {
                changeSET.push(changes[j]);
            }
        }
        //alert( changeSET.length);
        return changeSET;
    }

    this.UpdateAll_OBR_Items = function (msh_id, daObRIndex, objectContext, healthCondition) {
        var changeSET = [];
        var arrayLength = this.data.ObrList[daObRIndex].ObxList === null ? 0 : this.data.ObrList[daObRIndex].ObxList.length;
        if (arrayLength <= 0) {
            this.data.ObrList[daObRIndex].HCAssignment = healthCondition;
            this.data.ObrList[daObRIndex].ShowCancel = true;
            var change = {
                "NodeType": "2",
                "MSH_ID": msh_id,
                "OBR_ID": this.data.ObrList[daObRIndex].OBR_ID,
                "OBX_ID": "NONE",
                "HealthCondition": healthCondition
            }
            changeSET.push(change);
        }
        else {

            for (var i = 0; i < arrayLength; i++) {
                var change = this.Update_HC_Single_OBX(msh_id, this.data.ObrList[daObRIndex].OBR_ID,
                                    this.data.ObrList[daObRIndex].ObxList[i], objectContext, healthCondition);
                changeSET.push(change);
            }
        }
        return changeSET;
    }

    this.Update_HC_Single_OBX = function (msh_id, obr_id, obx, objectContext, healthCondition) {
        obx.HCAssignment = healthCondition;
        obx.ShowCancel = true;
        var change = {
            "NodeType": "3",
            "MSH_ID": msh_id,
            "OBR_ID": obr_id,
            "OBX_ID": obx.OBX_ID,
            "HealthCondition": healthCondition
        }

        return change;
        //alert(obx.OBX_ID);
    }

    this.Save = function (msh_id, successCallback, failureCallback) {

        var ret = this.Validate();
        if (ret.Success === true) {

            var token = $('input[name="__RequestVerificationToken"]').val();

            daFactory.SaveData(token, vm.data.MSH_ID, vm.data.Note, vm.data.AllChanges).success(function (response) {
                var ResponseData = { "MSH_ID": msh_id, "ChangeSet": vm.data.AllChanges, "ResponseCode": response };
                successCallback(ResponseData);
            }).error(function (data, status, headers, config) {
                if (null !== failureCallback) {
                    failureCallback(status + " : " + data);
                }
            })
        }
        else {
            failureCallback(ret.ErrorMessage);
        }
    }
    this.Validate = function () {
        var result = {
            "Success": true,
            "ErrorMessage": ""
        };

        vm.data.ObrList.forEach(function (obr) {

            if (obr.ObxList === null || obr.ObxList.length === 0) {

                if (obr.HCAssignment == null || obr.HCAssignment.length <= 0) {
                    result.Success = false;
                    result.ErrorMessage = "OBR (" + obr.OBR_ID + ") is unassigned";
                }
            }
            else {
                for (var i = 0; i < obr.ObxList.length; i++) {
                    if (obr.ObxList[i].HCAssignment == null || obr.ObxList[i].HCAssignment.length <= 0) {
                        result.Success = false;
                        result.ErrorMessage = result.ErrorMessage + "OBX (" + obr.ObxList[i].OBX_ID + " ) is unassigned\n";
                    }
                }
            }
        });

        return result;
    }

    this.Remove_HC_Single_OBX = function (msh_id, obr_id, obx) {
        obx.HCAssignment = "";
        obx.ShowCancel = false;
        var change = {
            "NodeType": "3",
            "MSH_ID": msh_id,
            "OBR_ID": obr_id,
            "OBX_ID": obx.OBX_ID,
            "HealthCondition": ""
        }
        return change;
    }

    this.Remove_HC_Single_OBR = function (msh_id, obr, objectContext) {
        obr.HCAssignment = "";
        obr.ShowCancel = false;
        var change = {
            "NodeType": "2",
            "MSH_ID": msh_id,
            "OBR_ID": obr.OBR_ID,
            "OBX_ID": "NONE",
            "HealthCondition": ""
        }
        return change;
    }

    this.GetObrIndex = function (obrid) {

        var arrayLength = this.data.ObrList.length;
        for (var i = 0; i < arrayLength; i++) {
            if (obrid === this.data.ObrList[i].OBR_ID) {
                return i;
            }
        }
    }

    this.GetObxIndex = function (daObRIndex, obxid) {

        var arrayLength = this.data.ObrList[daObRIndex].ObxList.length;
        for (var i = 0; i < arrayLength; i++) {
            if (obxid === this.data.ObrList[daObRIndex].ObxList[i].OBX_ID) {
                return i;
            }
        }
    }
});
