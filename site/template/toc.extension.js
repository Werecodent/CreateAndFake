/**
 * This method will be called at the start of exports.transform in toc.html.js and toc.json.js
 */
exports.preTransform = function (model) {
    simplifyTocNamespaces(model);
    return model;

    /**
     * Cleans up titles in the table of contents by removing the common project namespace.
     * @param {*} item Model with a potential title name to clean.
     */
    function simplifyTocNamespaces(item) {
        if (item.name) {
            if (item.name == "Werecodent.CreateAndFake") {
                item.name = "CreateAndFake";
            } else {
                item.name = item.name.replace("Werecodent.CreateAndFake.", '');
            }
        }

        if (item.items) {
            var length = item.items.length;
            for (var i = 0; i < length; i++) {
                simplifyTocNamespaces(item.items[i]);
            };
        }
    }
}
