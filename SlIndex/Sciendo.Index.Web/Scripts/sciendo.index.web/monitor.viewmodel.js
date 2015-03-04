    function monitorViewModel() {
        var self = this;
        self.hub = $.connection.monitoringHub;
        self.monitoringMessages = ko.observableArray([]);
        self.maximumMonitoringMessagesDisplay = ko.observable(10);
        //Initializes the view model
        self.init = function() {
            this.hub.server.send();
        }

        self.hub.client.addNewMessageToPage = function (message) {
            if (self.monitoringMessages().length > self.maximumMonitoringMessagesDisplay())
                self.monitoringMessages.removeAll();
            self.monitoringMessages.push(message);
        }
    }

// Initiate the Knockout bindings
    var vm = new monitorViewModel();
    ko.applyBindings(vm);
// Start the connection
    $.connection.hub.start().done(function() { vm.init(); });
