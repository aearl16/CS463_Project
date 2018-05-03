/**
 * Scripts for populating the format tags on the _Menu partial view
 * and all other partial views derived from it
 *
 * For the purpose of this code, the following information about the
 * models is relevant:
 * 1. A FormatTag is a tag that identifies the type of writing based on
 *    format rather than genre or subject matter (i.e. novel rather than
 *    romance)
 * 2. Each FormatTag has a primary key int FormatID, a string FormatName,
 *    and a string Explanation of the format
 * 3. A FormatCategory is a connection between two different FormatTags
 *    where one is the parent of another
 * 4. FormatCategory is a separate table with its own individual primary
 *    key int FormatCategoryID, a foreign key int FormatID that represents
 *    the child format, a foreign key int (FormatID) renamed as ParentID
 *    that represents the parent of said child format, and a nullable
 *    foreign key int (FormatID) renamed as SecondaryParentID that represents
 *    a secondary parent that is a required parent, if one exists
 * 5. Using the information from FormatCategory, FormatTag additionally has a
 *    list of FormatCategory where the ParentID is the same as FormatTag's
 *    primary key; this list is called ChildFormats
 *
 * The intended behavior created by this script is as follows:
 * 1. When moused over, a format tag should give an explanation of its meaning.
 * 2. The only format tags to show up initially should be those without parents.
 * 3. When the checkbox for a format tag is checked, it should load that tag's
 *    immediate child formats.
 * 4. If a format tag has two parents, it should only load when the checkboxes 
 *    for both parents are checked.
 * 5. When the checkbox for a format tag is unchecked, it should unload that tag's
 *    immediate children unless one of those said children's checkbox is checked,
 *    in which case, all children without a checked checkbox should be unloaded
 *    and the child(ren) with (a) checked checkbox(es) should remain.
 * 6. "Loaded" is not being used literally. The content of the format tag container
 *    shouldn't be repopulated every time a checkbox is checked or unchecked, it should
 *    just appear that way to the end user through the use of things like display: hidden,
 *    the collapse class, etc.
 */

/**
 * Empties the field for the format tag description and appends the explanation
 * for the current format tag
 * @param {string} explanation (FormatID's Explanation from the partial view)
 */
function showExplanation(explanation) {
    $("#formatTagDescription").empty();
    $("#formatTagDescription").append(explanation);
}
  
/**
 * A function to check whether the format tag's checkbox that has just been
 * changed was just checked or unchecked and then call the proper function to
 * load or unload children
 * 
 * @param {int} id (FormatID for current format tag from partial view)
 * @param {int[]} sdChildren (FormatIDs for children of current format tag that have no secondary parent)
 * @param {int[]} mdChildren (FormatIDs for children of current format tag that do have a secondary parent)
 * @param {int[]} dependencies (SecondaryParentIDs for children of current format tag that have a secondary parent)
 * @param {int[]} mpChildren (FormatIDs for children with more than one possible parent other than id; each FormatID is listed once for each additional parent)
 * @param {int[]} altParents (FormatID for each parent other than id; indexes match mpChildren)
 * sd stands for singular dependency and md stands for multiple dependency
 * mp stands for multiple parent; this is different than md because
 */
function ftChildren(id, sdChildren, mdChildren, dependencies, mpChildren, altParents) {
    //if the checkbox for the format tag with a FormatID of id was checked
    if ($("#formatTagContainer span." + id + " input[type=checkbox]").is(':checked')) {
        //load all the singular dependency children and load any multi-dependency children with their other dependency checked
        loadChildren(sdChildren, mdChildren, dependencies);
    } 
    else { //if the checkbox was unchecked, you don't need to worry about dependencies
        //for each singular dependency child
        for (var i = 0; i < sdChildren.length; i++) {
            //for each singular dependency child where the child has more than one possible parent
            for (var j = 0; j < mpChildren.length; j++) {
                //if the singular dependency child and the multiple possible parent child are the same
                if (sdChildren[i] === mpChildren[j]) {
                    //remove the singular depency child from the array
                    sdChildren.splice(i, 1);
                }
            }
        }

        //add all of the multiple dependency children to the same array as the single dependency children
        for (i = 0; i < mdChildren.length; i++) 
            sdChildren.push(mdChildren[i]);

        console.log("mdChildren: " + mdChildren);
        console.log("mpChildren: " + mpChildren);
        console.log("sdChildren should overlap with mdChildren, but not mpChildren: " + sdChildren);

        //call the function to unload all of the singular dependency children with only one possible parent
        //and the multiple dependency children with only one possible combination of parents;
        //remove the children with multiple possible parents only if none of the parents are selected
        unloadChildren(sdChildren, mpChildren, altParents);
    }
}

/**
 * A function to load the correct children of a format tag that has been checked.
 * @param {int[]} sdChildren (FormatIDs for children of current format tag that have no secondary parent)
 * @param {int[]} mdChildren (FormatIDs for children of current format tag that do have a secondary parent)
 * @param {int[]} dependencies (SecondaryParentIDs for children of current format tag that have a secondary parent)
 * sd stands for singular dependency and md stands for multiple dependency
 */
function loadChildren(sdChildren, mdChildren, dependencies) {
    //for each singular dependency child
    for (var i = 0; i < sdChildren.length; i++) {
        //if the singular dependency child is collapsed, uncollapse it
        if ($("#formatTagContainer span." + sdChildren[i]).hasClass("collapse"))
            $("#formatTagContainer span." + sdChildren[i]).removeClass("collapse");
    }

    //for each multiple dependency child
    for (i = 0; i < mdChildren.length; i++) {
        //if the other tag it's dependent on is already selected
        if ($("#formatTagContainer span." + dependencies[i] + " input[type=checkbox]").is(':checked')) {
            //if the multiple dependency child is currently collapsed, uncollapse it
            if ($("#formatTagContainer span." + mdChildren[i]).hasClass("collapse"))
                $("#formatTagContainer span." + mdChildren[i]).removeClass("collapse");
        }
    }
}

function unloadChildren(sdChildren, mpChildren, altParents) {
    for (var i = 0; i < sdChildren.length; i++) {
        if ($("#formatTagContainer span." + sdChildren[i] + " input[type=checkbox]").is(':checked') !== true) {
            if ($("#formatTagContainer span." + sdChildren[i]).hasClass("collapse") !== true)
                $("#formatTagContainer span." + sdChildren[i]).addClass("collapse");
        }
    }

    var current = mpChildren[0];
    var parentChecked = false;
    for (i = 0; i < mpChildren.length; i++) {
        if ((i + 1) < mpChildren.length && mpChildren[i + 1] === current) {
            if (parentChecked !== true) {
                if ($("#formatTagContainer span." + altParents[i] + " input[type=checkbox]").is(':checked')) {
                    parentChecked = true;
                }
            }
        }
        else if (mpChildren[i] === current) {
            if (parentChecked !== true) {
                if ($("#formatTagContainer span." + altParents[i] + " input[type=checkbox]").is(':checked') !== true) {
                    if ($("#formatTagContainer span." + mpChildren[i]).hasClass("collapse") !== true)
                        $("#formatTagContainer span." + mpChildren[i]).addClass("collapse");
                }
            }
        }
        else {
            parentChecked = false;
            current = mpChildren[i];

            if ($("#formatTagContainer span." + altParents[i] + " input[type=checkbox]").is(':checked')) {
                parentChecked = true;
            }
            else if ((i + 1) === mpChildren.length || mpChildren[i + 1] !== current) {
                if ($("#formatTagContainer span." + mpChildren[i]).hasClass("collapse") !== true)
                    $("#formatTagContainer span." + mpChildren[i]).addClass("collapse");
            }
        }
    }
}