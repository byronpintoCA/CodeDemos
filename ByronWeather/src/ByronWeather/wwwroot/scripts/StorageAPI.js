cookieAPI = {

    setJson: function (name, value, days) {
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            var expires = "; expires=" + date.toGMTString();
        }
        else
            var expires = "";
        document.cookie = name + "=" + JSON.stringify(value) + expires + "; path=/";
    },

    getJson: function (name) {
        var nameEQ = name + "=",
            ca = document.cookie.split(';');

        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0)
                return JSON.parse(c.substring(nameEQ.length, c.length));
        }

        return null;
    },
    setString: function (name, value, days) {
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            var expires = "; expires=" + date.toGMTString();
        }
        else
            var expires = "";
        document.cookie = name + "=" + value + expires + "; path=/";
    },
    getString: function (name) {
        var nameEQ = name + "=",
            ca = document.cookie.split(';');

        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0)
                return c.substring(nameEQ.length, c.length);
        }

        return null;

    }
}

localStorageAPI = {

    isSupported: function () {

        if (window.localStorage) {
            return true;
        }
        else {
            return false;
        }

        // return window.localStorage;
    },

    setItem: function (key, value) {
        return localStorage.setItem(key, value);
    },

    getItem: function (key) {
        return localStorage.getItem(key);
    },

    setObject: function (key, object) {
        return localStorage.setItem(key, JSON.stringify(object));
    },

    getObject: function (key) {
        var a = localStorage.getItem(key);

        return JSON.parse(a);
        //return JSON.parse);
    },

    removeItem: function (key) {
        return localStorage.removeItem(key);
    },

    clearAll: function () {
        return localStorage.clear();
    }

};

LocationHelperAPI = {

    storageKey: "LocationData",

    saveALocation: function (display, latitude, longitude) {

        var newLocation = { "display": display, "latitude": latitude, "longitude": longitude };

        var data = this.getLocationData();


        if (data === null) {
            data = { Locations: {} }
        }

        var arrLocations = $.map(data.Locations, function (el) { return el; })
        arrLocations.push(newLocation);
        //arrLocations.unshift(newLocation);
        data.Locations = arrLocations;

        this.setLocationData(data);

    },

    removeLocation: function (display, latitude, longitude) {

        var data = this.getLocationData();

        if (data === null) return;
        var arrLocations = $.map(data.Locations, function (el) { return el; })

        var newData = arrLocations.filter(function (obj) {

            if (obj.display === display && obj.latitude === latitude && obj.longitude === longitude) {
                return false;
            }
            else {
                return true;
            }

        });

        data.Locations = newData;
        this.setLocationData(data);

    },

    getLocationData: function () {

        if (localStorageAPI.isSupported() === true) {

            return localStorageAPI.getObject(this.storageKey);
        }
        else {
            return cookieAPI.getJson(this.storageKey);
        }
    },

    setLocationData: function (data) {

        if (localStorageAPI.isSupported() === true) {

            localStorageAPI.setObject(this.storageKey, data);
        }
        else {
            cookieAPI.setJson(this.storageKey, data, 999999)
        }
    },

    clearLocationData: function () {
        var data = { Locations: {} }

        if (localStorageAPI.isSupported() === true) {

            localStorageAPI.removeItem(this.storageKey);
        }
        else {
            cookieAPI.setJson(this.storageKey, data, 999999)
        }
    }

}
