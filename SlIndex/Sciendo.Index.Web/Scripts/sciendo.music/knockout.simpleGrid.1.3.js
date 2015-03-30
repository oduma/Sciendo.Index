(function () {
    // Private function
    function getColumnsForScaffolding(data) {
        if ((typeof data.length !== 'number') || data.length === 0) {
            return [];
        }
        var columns = [];
        for (var propertyName in data[0]) {
            columns.push({ headerText: propertyName, rowText: propertyName });
        }
        return columns;
    }

    ko.simpleGrid = {
        // Defines a view model class you can use to populate a grid
        viewModel: function (configuration, dataContextInstance) {
            this.data = configuration.data;

            // If you don't specify columns configuration, we'll use scaffolding
            this.columns = configuration.columns || getColumnsForScaffolding(ko.utils.unwrapObservable(this.data));

            this.keyColumn = configuration.keyColumn;

            this.linkColumn = configuration.linkColumn;

            queue= function(itemId,e)
            {
                dataContextInstance.addToQueue((e.srcElement.id != "")?e.srcElement.id:e.srcElement.parentElement.id);
            }

        }
    };

    // Templates used to render the grid
    var templateEngine = new ko.nativeTemplateEngine();

    templateEngine.addTemplate = function(templateName, templateMarkup) {
        document.write("<script type='text/html' id='" + templateName + "'>" + templateMarkup + "<" + "/script>");
    };

    templateEngine.addTemplate("ko_simpleGrid_grid", "\
                    <table class=\"ko-grid\" cellspacing=\"10px\">\
                        <thead class=\"ko-grid-header\">\
                            <tr data-bind=\"foreach: columns\">\
                                <!-- ko ifnot:isKey-->\
                                    <th  class=\"ko-grid-header\" data-bind=\"style:{width: colWidth}, text: headerText\"></th>\
                                <!-- /ko -->\
                            </tr>\
                        </thead>\
                        <tbody data-bind=\"foreach: data\">\
                           <tr data-bind=\"foreach: $parent.columns\">\
                                <!-- ko ifnot:$root.keyColumn==rowText-->\
                                    <!-- ko if:isLink-->\
                                        <td>\
                                        <a href=\"#\" data-bind=\"click:queue, attr:{'id': $parent[$root.keyColumn] }, html: typeof rowText == 'function' ? rowText($parent) : $parent[rowText]\"/></td>\
                                    <!-- /ko -->\
                                    <!-- ko ifnot:isLink-->\
                                        <td data-bind=\"html: typeof rowText == 'function' ? rowText($parent) : $parent[rowText] \"></td>\
                                    <!-- /ko -->\
                                <!-- /ko -->\
                            </tr>\
                        </tbody>\
                    </table>");
    // The "simpleGrid" binding
    ko.bindingHandlers.simpleGrid = {
        init: function() {
            return { 'controlsDescendantBindings': true };
        },
        // This method is called to initialize the node, and will also be called again if you change what the grid is bound to
        update: function (element, viewModelAccessor, allBindingsAccessor) {
            var viewModel = viewModelAccessor(), allBindings = allBindingsAccessor();

            // Empty the element
            while(element.firstChild)
                ko.removeNode(element.firstChild);

            // Allow the default templates to be overridden
            var gridTemplateName      = allBindings.simpleGridTemplate || "ko_simpleGrid_grid";

            // Render the main grid
            var gridContainer = element.appendChild(document.createElement("DIV"));
            ko.renderTemplate(gridTemplateName, viewModel, { templateEngine: templateEngine }, gridContainer, "replaceNode");
        }
    };
})();