/*!
 * Utilities to help with UI related code
 */

function foo() {
    return "SSSS";
}
function fillDropdownWithResponse(url, DropdownlistId) {
    /// <summary> gets a Json array in this form: https://select2.org/data-sources/arrays
    /// and loads it in the select2 (https://select2.org) drop-down list </summary>
    /// <param name="url" type="string">the GET url of the action</param>  
    /// <param name="DropdownlistId" type="string">the id of the dropdownlist to be filled</param>  
    $.ajax({
        url: url,
        type: 'get',
        success: function (result) {
            $(DropdownlistId).select2({
                data: result
            });
        },
    });
}