// start PolyFills

if (!String.prototype.startsWith) {
    String.prototype.startsWith = function (searchString, position) {
        return this.substr(position || 0, searchString.length) === searchString;
    };
}

// end PolyFills


var _MainData;
var _ChangeSet = null;
var old_MSH_ID = null;
var not_Set_MSH_ID = true;

var _panelCollapsed = true;

$(function () {

    _hcaScope.vm.loadAllData(function (data) {

        _MainData = data;
        TreeViewLoad('#MyTree', function () { return _MainData.TreeData; }, false, NodeSelected, null, NodeExpanded, NodeCollapsed);

        SetViewBasedOnData();
        $('#loadAnimation').hide();
        setTimeout(function () {
            $('#MyTree').treeview('selectNode', [0, { silent: false }]);
        }, 50);


    }, function (data, status, headers, config) {
        alert("Unable to load Data : Response code : " + status);
        $('#loadAnimation').hide();
    });


    $('.panel').on('hidden.bs.collapse', function (e) {
        if (e.target.id === "HL7_Panel" && e.currentTarget.className.startsWith("panel dummy") === true) {
            _panelCollapsed = true;
            _hcaScope.$apply(function () {
                _hcaScope.vm.loadHL7Data(_panelCollapsed);
            });
        }
    }).on('show.bs.collapse', function (e) {
        if (e.target.id === "HL7_Panel" && e.currentTarget.className.startsWith("panel dummy") === true) {
            _panelCollapsed = false;
            _hcaScope.$apply(function () {
                _hcaScope.vm.loadHL7Data(_panelCollapsed);
            });
        }
    });

});

function SetViewBasedOnData() {
    if (_MainData.TreeData.length <= 0) {
        $("#QueueContentSection").hide();
        $("#NoDataSection").show();
        $("#NoDataText").text("No data to process ..")
    }
    else {
        $("#QueueContentSection").show();
        $("#NoDataSection").hide();
    }
}

function NodeExpanded(event, node) {
    UpdateTreeViewData(node, true);
}

function NodeCollapsed(event, node) {
    UpdateTreeViewData(node, false);
}

function UpdateTreeViewData(node, IsExpanded) {
    for (var i = 0; i < _MainData.TreeData.length; i++) {
        RecordNodeState(_MainData.TreeData[i], node, IsExpanded);
    }
}

function RecordNodeState(TargetNode, eventNode, IsExpanded) {
    if (TargetNode.nodeUniqueID === eventNode.nodeUniqueID) {
        TargetNode.state.expanded = IsExpanded;
    }
    else {
        $.each(TargetNode.nodes, function (index, value) {
            RecordNodeState(value, eventNode, IsExpanded);
        });
    }
}

function NodeSelected(event, node) {

    var theDataObject = getCorrectObject(node);

    CheckIfAutoCompleteNeedsTobeRemoved(theDataObject);
    _hcaScope.$apply(function () {
        _hcaScope.vm.setVM(theDataObject, _panelCollapsed);
    });

    CheckIfAutoCompleteSetupIsRequired(theDataObject);

    setTimeout(function () {
        var elementID;
        switch (node.type) {
            case 1:
                elementID = "#DaHl7Button";
                $(elementID)[0].scrollIntoView(false);
                break;
            case 2:
                elementID = "#Scroll_OBR_" + node.msh_id + "_" + node.obr_id;
                $(elementID)[0].scrollIntoView(true);

                elementID = "#MainHCAUrl";
                $(elementID)[0].scrollIntoView(true);

                break;
            case 3:
                elementID = "#Scroll_OBX_" + node.msh_id + "_" + node.obr_id + "_" + node.obx_id;
                $(elementID)[0].scrollIntoView(true);

                elementID = "#MainHCAUrl";
                $(elementID)[0].scrollIntoView(true);
                break;
        }

    }, 100);

}

function getCorrectObject(node) {
    var o = $.grep(_MainData.MSHData, function (item) {

        if (item.MSH_ID === node.msh_id) {
            return item;
        }
    });
    return o[0];
}

//Auto Complete Related - Start 
function CheckIfAutoCompleteNeedsTobeRemoved(newObject) {
    if (old_MSH_ID !== null) {
        if (old_MSH_ID !== newObject.MSH_ID) {
            RemoveExistingAutoComplete();
            not_Set_MSH_ID = true;
        }
    }
}

function CheckIfAutoCompleteSetupIsRequired(newObject) {
    if (not_Set_MSH_ID === true) {
        old_MSH_ID = newObject.MSH_ID;
        SetupAutoComplete();
        not_Set_MSH_ID = false;
    }
}

function RemoveExistingAutoComplete() {

    //alert('Remove');

    $("input[type=search]").each(function (index, elem) {

        if (elem.id.startsWith("HCA-Search") === true) {
            //alert("Remove#" + elem.id);
            $("#" + elem.id).autocomplete("destroy");
        }
    });

}

function SetupAutoComplete() {
    //alert('Add');
    setTimeout(function () {
        $("input[type=search]").each(function (index, elem) {

            if (elem.id.startsWith("HCA-Search") === true) {
                //alert("ADD#" + elem.id);
                $("#" + elem.id).autocomplete({
                    source: _MainData.AssignCodes,
                    select: AutoCompleteSelect,
                    minLength: 0
                });
            }
        });

    }, 300);
}

function ExtractObjectContext(id) {
    var objectContext = {
        "NodeType": "",
        "MSH_ID": "",
        "OBR_ID": "",
        "OBX_ID": ""
    }

    var arr = id.split("_");

    for (i = 0; i < arr.length; i++) {

        switch (i) {
            case 1:
                objectContext.NodeType = arr[i];
                break;
            case 2:
                objectContext.MSH_ID = arr[i];
                break;
            case 3:
                objectContext.OBR_ID = arr[i];
                break;
            case 4:
                objectContext.OBX_ID = arr[i];
                break;
            default:
        }
    }

    return objectContext;
}

function AutoCompleteSelect(sender, element) {
    //alert(sender.target.id + " : " + element.item.label); // element.item.value

    var objectContext = ExtractObjectContext(sender.target.id);
    _hcaScope.$apply(function () {
        UpdateChangeSet(_hcaScope.vm.AddHC(objectContext, element.item));
    });

    //sender.target.val("sdfsd");
    setTimeout(function () {
        $("#" + sender.target.id).val("");
    }, 100);

    UpdateTreeViewFromChangeset(true);
    TreeViewLoad('#MyTree', function () { return _MainData.TreeData; }, true, NodeSelected, null, NodeExpanded, NodeCollapsed);
    _hcaCartScope.$apply(function () {
        _hcaCartScope.cartVM.setCart(_ChangeSet);
    });
    SetCartCount();
}

function SetCartCount() {
    $("#ShoppingCart").text(" " + _ChangeSet.length);
}

function UpdateChangeSet(changes) {
    if (_ChangeSet === null) {
        _ChangeSet = changes;
    }
    else {
        for (var i = 0; i < changes.length; i++) {
            var updated = false;
            for (var j = 0; j < _ChangeSet.length; j++) {

                if (_ChangeSet[j].NodeType === changes[i].NodeType &&
                    _ChangeSet[j].MSH_ID === changes[i].MSH_ID &&
                    _ChangeSet[j].OBR_ID === changes[i].OBR_ID &&
                    _ChangeSet[j].OBX_ID === changes[i].OBX_ID) {
                    _ChangeSet[j].HealthCondition = changes[i].HealthCondition;
                    updated = true;
                }
            }
            if (false === updated) {
                _ChangeSet.push(changes[i]);
            }
        }
    }
}

function UpdateTreeViewFromChangeset() {
    //alert('Lets update the treeview - ' + _ChangeSet.length);
    //alert(_MainData.TreeConfig.DefaultBackColor + " : " + _MainData.TreeConfig.ChangeBackColor);
    for (var i = 0; i < _MainData.TreeData.length; i++) {
        var relatedItems = ForMSHGetRelatedChangeSetItems(_MainData.TreeData[i]);
        if (relatedItems.length > 0) {
            UpdateChildNodes(_MainData.TreeData[i].nodes, relatedItems);
        }
    }
}
function UpdateChildNodes(ObrLevelNodes, relatedItems) {

    for (var i = 0; i < ObrLevelNodes.length; i++) {
        var changeItems = ForOBRGetRelatedChangeSetItems(ObrLevelNodes[i], relatedItems);
        if (changeItems.length > 0) {
            ProcessOBXs(ObrLevelNodes[i], changeItems, true);
        }
    }
}

function ProcessOBXs(ObrNode, relatedItems, adding) {

    if (ObrNode.nodes.length <= 0) {
        SetNodeState(ObrNode, adding);
    }
    else {
        var ObXLevelNodes = ObrNode.nodes;
        for (var i = 0; i < ObXLevelNodes.length; i++) {
            for (var j = 0; j < relatedItems.length; j++) {

                if (ObXLevelNodes[i].obx_id === relatedItems[j].OBX_ID) {
                    SetNodeState(ObXLevelNodes[i], adding);
                }
            }
        }
    }
}

function SetNodeState(node, changed) {
    if (changed === true) {
        node.backColor = _MainData.TreeConfig.ChangeBackColor;;
        node.tags = ["*"];
    }
    else {
        node.backColor = _MainData.TreeConfig.DefaultBackColor;
        node.tags = null;
    }

}

function ForOBRGetRelatedChangeSetItems(ObrLevelNode, relatedItems) {

    var result = [];
    for (var i = 0; i < relatedItems.length; i++) {
        if (ObrLevelNode.obr_id === relatedItems[i].OBR_ID) {
            result.push(relatedItems[i]);
        }
    }
    return result;
}

function ForMSHGetRelatedChangeSetItems(topLevelNode) {

    var result = [];
    for (var i = 0; i < _ChangeSet.length; i++) {
        if (topLevelNode.msh_id === _ChangeSet[i].MSH_ID) {
            result.push(_ChangeSet[i]);
        }
    }
    return result;
}

function RemoveAllHC() {
    _hcaScope.$apply(function () {
        var changes = _hcaScope.vm.RemoveAllHC();
        UpdateViewFromRemovalList(changes, true);
    });
}

function RemoveObrHC(context) {
    _hcaScope.$apply(function () {
        var objectContext = ExtractObjectContext(context.id);
        var changes = _hcaScope.vm.RemoveAllHC(objectContext);
        UpdateViewFromRemovalList(changes, true);
    });
}

function RemoveHCAssignment(context) {
    _hcaScope.$apply(function () {
        var objectContext = ExtractObjectContext(context.id);
        var changes = _hcaScope.vm.RemoveHealth(objectContext);
        UpdateViewFromRemovalList(changes, true);
    });
}

function Save(context) {

    var ctx = ExtractObjectContext(context.id);

    _hcaScope.vm.Save(ctx.MSH_ID, function (response) {

        if (response.ResponseCode.Success === 1) {

            _MainData.TreeData = $.grep(_MainData.TreeData, function (rootNodes) {
                return rootNodes.msh_id === ctx.MSH_ID;
            }, true);

            UpdateViewFromRemovalList(response.ChangeSet, false);

            setTimeout(function () {
                $('#MyTree').treeview('selectNode', [0, { silent: false }]);
            }, 50);

        }
        else {
            alert("Failed to save .. Server responded :" + response.ResponseCode.ErrorMessage)
        }

    }, function (error) {
        alert(error);
    });

}

function UpdateViewFromRemovalList(changes, silentMode) {

    RemoveItemsFromChangeSet(changes);
    RemoveItemsFromTreeView(changes);

    TreeViewLoad('#MyTree', function () { return _MainData.TreeData; }, silentMode, NodeSelected, null, NodeExpanded, NodeCollapsed);
    //_hcaCartScope.$apply(function () {
    _hcaCartScope.cartVM.setCart(_ChangeSet);
    //});

    SetCartCount();
    SetViewBasedOnData();

}

function RemoveItemsFromTreeView(changes) {
    changes.forEach(function (theChange) {

        for (var i = 0; i < _MainData.TreeData.length; i++) {

            if (_MainData.TreeData[i].msh_id === theChange.MSH_ID) {

                for (var j = 0; j < _MainData.TreeData[i].nodes.length; j++) { // ObRs

                    if (_MainData.TreeData[i].nodes[j].obr_id === theChange.OBR_ID) {
                        ProcessOBXs(_MainData.TreeData[i].nodes[j], changes, false);
                    }
                }
            }
        }
    });
}

function RemoveItemsFromChangeSet(changes) {

    for (var i = 0; i < changes.length; i++) {
        for (var j = 0; j < _ChangeSet.length; j++) {

            if (_ChangeSet[j].NodeType === changes[i].NodeType &&
                _ChangeSet[j].MSH_ID === changes[i].MSH_ID &&
                _ChangeSet[j].OBR_ID === changes[i].OBR_ID &&
                _ChangeSet[j].OBX_ID === changes[i].OBX_ID) {
                _ChangeSet.splice(j, 1);

            }
        }
    }
}
