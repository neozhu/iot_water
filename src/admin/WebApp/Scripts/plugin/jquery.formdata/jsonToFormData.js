(function(root, factory) {
    if (typeof define === "function" && define.amd) {
        define([], function() {
            return root.jsonToFormData = factory();
        });
    } else if (typeof module === "object" && module.exports) {
        module.exports = root.jsonToFormData = factory();
    } else {
        root.jsonToFormData = factory();
    }
})(this, function() {
    function mergeObjects(object1, object2) {
        return [ object1, object2 ].reduce(function(carry, objectToMerge) {
            Object.keys(objectToMerge).forEach(function(objectKey) {
                carry[objectKey] = objectToMerge[objectKey];
            });
            return carry;
        }, {});
    }
    function isArray(val) {
        return {}.toString.call(val) === "[object Array]";
    }
    function isJsonObject(val) {
        return !isArray(val) && typeof val === "object" && !!val && !(val instanceof Blob) && !(val instanceof Date);
    }
    function convert(jsonObject, options) {
        var defaultOptions = {
            showLeafArrayIndexes: true,
            includeNullValues: false,
            mapping: function(value) {
                if (typeof value === "boolean") {
                    return +value ? "1" : "0";
                }
                return value;
            }
        };
        var mergedOptions = mergeObjects(defaultOptions, options || {});
        return convertRecursively(jsonObject, mergedOptions);
    }
    function convertRecursively(jsonObject, options, parentKey, carryFormData) {
        var formData = carryFormData || new FormData();
        var index = 0;
        for (var key in jsonObject) {
            if (jsonObject.hasOwnProperty(key)) {
                var propName = parentKey || key;
                var value = options.mapping(jsonObject[key]);
                if (parentKey && isJsonObject(jsonObject)) {
                    propName = parentKey + "[" + key + "]";
                }
                if (parentKey && isArray(jsonObject)) {
                    if (isArray(value) || options.showLeafArrayIndexes) {
                        propName = parentKey + "[" + index + "]";
                    } else {
                        propName = parentKey + "[]";
                    }
                }
                if (isArray(value) || isJsonObject(value)) {
                    convertRecursively(value, options, propName, formData);
                } else if (value instanceof FileList) {
                    for (var j = 0; j < value.length; j++) {
                        formData.append(propName + "[" + j + "]", value.item(j));
                    }
                } else if (value instanceof Blob) {
                    formData.append(propName, value, value.name);
                } else if (value instanceof Date) {
                    formData.append(propName, value.toISOString());
                } else if ((value === null && options.includeNullValues || value !== null) && value !== undefined) {
                    formData.append(propName, value);
                }
            }
            index++;
        }
        return formData;
    }
    return convert;
});