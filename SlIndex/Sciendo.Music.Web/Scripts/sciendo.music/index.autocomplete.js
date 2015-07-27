var cache = {};
var lastTerm;

var createAutocomplete = function () {
    var self = $(this);
    var options = {
        source: function( request, response ) {
            var term = request.term;
            if (request.term.indexOf("\\", request.term.length - "\\".length) !== -1) {
                $.ajaxSetup({ "cache": false });
                $.getJSON(self.attr("data-autocomplete"), request, function (data, status, xhr) {
                    //cache[term] = data;
                    lastTerm = term;
                    response(data);
                });
            }
            else {
                response(cache[lastTerm]);
            }
        },
        minLength: 2,
        select: function(event, ui) {
            vm.selectValue(event.target.name, ui.item.value);
        }
    };
    self.autocomplete(options);
}

$(document).ready(function () {
    $("input[data-autocomplete]").each(createAutocomplete);
});
