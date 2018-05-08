/**
 * Scripts for populating the format tags on the _Menu partial view
 * and all the other partial views derived from it.
 */

/**
 * This function is for dealing with the top level genres, fiction and
 * nonfiction. It loads the children of whichever one is selected and then
 * unchecks and unloads any tags that require the writing to be of the opposite
 * top level genre
 * @param {string} name (name of the tag that has just been selected, either Fiction or Nonfiction)
 * @param {int[]} fictionOnly (genreIDs of genres that can only be fiction)
 * @param {int[]} nonfictionOnly (genreIDs of genres that can only be nonfiction)
 * @param {int[]} children (genreIDs of the children of the one that was just selected)
 */
function fictionOrNonfiction(name, fictionOnly, nonfictionOnly, children) {
    //if this is the first time either fiction or nonfiction is being selected, 
    //remove the collapse class from the rest of the genre tags
    if ($(".genreTagsThreePlus").hasClass("collapse"))
        $(".genreTagsThreePlus").removeClass("collapse");

    //for each of this genre's children
    for (var i = 0; i < children.length; i++) {
        //if the child is collapsed, uncollapse it
        if ($("#genreTagContainer span." + children[i]).hasClass("collapse"))
            $("#genreTagContainer span." + children[i]).removeClass("collapse");
    }

    //if fiction is the one that was just selected
    if (name === "Fiction") {
        //for each genre tag that is nonfiction only
        for (i = 0; i < nonfictionOnly.length; i++) {
            //if it isn't currently collapsed
            if ($("#genreTagContainer span." + nonfictionOnly[i]).hasClass("collapse") !== true) {
                //if it is currently checked, uncheck it
                if ($("#genreTagContainer span." + nonfictionOnly[i] + " input[type=checkbox]").is(":checked"))
                    $("#genreTagContainer span." + nonfictionOnly[i] + " input[type=checkbox]").checked = false;

                //collapse it
                $("#genreTagContainer span." + nonfictionOnly[i]).addClass("collapse");
            }

            //and let the checkbox know that it has been changed so the fuction on it will run
            $("#genreTagContainer span." + nonfictionOnly[i] + " input[type=checkbox]").toggle();
        }
    }
    else { //if nonfiction is the one that was just selected
        //for each genre tag that is fiction only
        for (i = 0; i < fictionOnly.length; i++) {
            //if it isn't already collapsed
            if ($("#genreTagContainer span." + fictionOnly[i]).hasClass("collapse") !== true) {
                //if it is currently checked, uncheck it
                if ($("#genreTagContainer span." + fictionOnly[i] + " input[type=checkbox]").is(":checked"))
                    $("#genreTagContainer span." + fictionOnly[i] + " input[type=checkbox]").checked = false;

                //collapse it
                $("#genreTagContainer span." + fictionOnly[i]).addClass("collapse");
            }

            //and let the checkbox know that it has been changed so the fuction on it will run
            $("#genreTagContainer span." + fictionOnly[i] + " input[type=checkbox]").toggle();
        }
    }
}

/**
 * 
 * @param {int} id (genreID of item that was just checked or unchecked)
 * @param {int[]} ndChildren (array of children without any dependencies)
 * @param {int[]} sdChildren (array of children with a single dependency)
 * @param {int[]} sDependencies (array of dependencies for sdChildren)
 * @param {int[]} mdChildren (array of children with multiple dependencies)
 * @param {int[]} mdfDependencies (array of the first dependencies for mdChildren)
 * @param {int[]} mdsDependencies (array of the second dependencies for mdChildren)
 * @param {int[]} mpndChildren (array of children with alternate parents and no dependencies)
 * @param {int[]} mpndParents (array of alternate parents for mpndChildren)
 * @param {int[]} mpsdChildren (array of children with alternate parents and a single dependency)
 * @param {int[]} mpsdParents (array of alternate parents for mpsdChildren)
 * @param {int[]} mpsDependencies (array of dependencies for mpsdChildren)
 * @param {int[]} fictionOnly (array of genres that can only be fiction)
 * @param {int[]} nonfictionOnly (array of genres that can only be nonfiction)
 */
function gtChildren(id, ndChildren, sdChildren, sDependencies, mdChildren, mdfDependencies, mdsDependencies, mpndChildren, mpndParents, mpsdChildren, mpsdParents, mpsDependencies, fictionOnly, nonfictionOnly) {
    //if the checkbox for the genre tag with a GenreID of id was checked
    if ($("#genreTagContainer span." + id + " input").is(":checked")) {
        //load all no dependency children and load any single dependency children with their other dependency checked and 
        //any multi-dependency children with their other two dependencies checked
        loadGenres(ndChildren, sdChildren, sDependencies, mdChildren, mdfDependencies, mdsDependencies);
    }
    else { //if we are unloading children, we only care about dependencies of mpsdChildren
        //for each no dependency child
        for (var i = 0; i < ndChildren.length; i++) {
            //for each no dependency child where the child has more than one possible parent
            for (var j = 0; j < mpndChildren.length; j++) {
                //if the no dependency child and the child with more than one possible parent are the same
                if (ndChildren[i] === mpndChildren[j]) {
                    //remove the no dependency child from the array
                    ndChildren.splice(i, 1);
                }
            }
        }

        //for each single dependency child
        for (i = 0; i < sdChildren.length; i++) {
            //for each single dependency child where the child has more than one possible parent
            for (j = 0; j < mpsdChildren.length; j++) {
                //if the single dependency child and the child with more than one possible parent are the same
                if (sdChildren[i] === mpsdChildren[j]) {
                    //remove the single dependency child from the array
                    sdChildren.splice(i, 1);
                }
            }
        }

        //add all the single dependency children to the same array as the no dependency children
        for (i = 0; i < sdChildren.length; i++)
            ndChildren.push(sdChildren[i]);

        //add all the multi-dependency children to the same array as the no dependency children
        for (i = 0; i < mdChildren.length; i++)
            ndChildren.push(mdChildren[i]);

        //call the function to unload all of the no dependency children and single dependency
        //and multi-dependency children with only one possible combination of parents; remove
        //the children with multiple possible parents or combination of parents only if none of
        //the parents or combination of parents are selected
        unloadGenres(ndChildren, mpndChildren, mpndParents, mpsdChildren, mpsdParents, mpsDependencies, fictionOnly, nonfictionOnly);
    }
}

function loadGenres(ndChildren, sdChildren, sDependencies, mdChildren, mdfDependencies, mdsDependencies) {
    //for each child without any dependencies
    for (var i = 0; i < ndChildren.length; i++) {
        //if the child is currently collapsed, uncollapse it
        if ($("#genreTagContainer span." + ndChildren[i]).hasClass("collapse"))
            $("#genreTagContainer span." + ndChildren[i]).removeClass("collapse");
    }

    //for each child with only one dependency
    for (i = 0; i < sdChildren.length; i++) {
        //if the dependency for this child is checked
        if ($("#genreTagContainer span." + sDependencies[i] + " input").is(":checked")) {
            //if the child is currently collapsed, uncollapse it
            if ($("#genreTagContainer span." + sdChildren[i]).hasClass("collapse"))
                $("#genreTagContainer span." + sdChildren[i]).removeClass("collapse");
        }
    }

    //for each child with two dependencies
    for (i = 0; i < mdChildren.length; i++) {
        //if both dependencies for this child are checked 
        if ($("#genreTagContainer span." + mdfDependencies[i] + " input").is(":checked") && $("#genreTagContainer span." + mdsDependencies[i] + " input").is(":checked")) {
            //if the child is currently collapsed, uncollapse it
            if ($("#genreTagContainer span." + mdChildren[i]).hasClass("collapse"))
                $("#genreTagContainer span." + mdChildren[i]).removeClass("collapse");
        }
    }
}

function unloadGenres(rChildren, mpndChildren, mpndParents, mpsdChildren, mpsdParents, mpsDependencies, fictionOnly, nonfictionOnly) {
    //for all of the children that we KNOW can be depopulated
    for (var i = 0; i < rChildren.length; i++) {
        //if the checkbox for this child is checked
        if ($("#genreTagContainer span." + rChildren[i] + " input").is(":checked")) {
            //if Fiction is currently selected
            if ($("#genreTagContainer span.1 input").is(":checked")) {
                //for each genre in nonfictionOnly
                for (var j = 0; j < nonfictionOnly.length; j++) {
                    //if the current item in rChildren is the same as the current item in nonfictionOnly
                    if (rChildren[i] === nonfictionOnly[j]) {
                        //uncheck the checkbox because it is no longer valid
                        $("#genreTagContainer span." + rChildren[i] + " input").checked = false;

                        //if it isn't already collapsed, collapse it
                        if ($("#genreTagContainer span." + rChildren[i]).hasClass("collapse") !== true)
                            $("#genreTagContainer span." + rChildren[i]).addClass("collapse");

                        //Call toggle to let the input know to run its onchange function
                        $("#genreTagContainer span." + rChildren[i] + " input").toggle();
                    }
                }
            }
            else { //if Nonfiction is currently selected
                //for each genre in fictionOnly
                for (j = 0; j < fictionOnly.length; j++) {
                    //if the current item in rChildren is the same as the current item in fictionOnly
                    if (rChildren[i] === fictionOnly[j]) {
                        //uncheck the checkbox because it is no longer valid
                        $("#genreTagContainer span." + rChildren[i] + " input").checked = false;

                        //if it isn't already collapsed, collapse it
                        if ($("#genreTagContainer span." + rChildren[i]).hasClass("collapse") !== true)
                            $("#genreTagContainer span." + rChildren[i]).addClass("collapse");

                        //Call toggle to let the input know to run its onchange function
                        $("#genreTagContainer span." + rChildren[i] + " input").toggle();
                    }
                }
            }
        }
        else {//if the checkbox for this child is not checked
            //if it isn't already collapsed, collapse it
            if ($("#genreTagContainer span." + rChildren[i]).hasClass("collapse") !== true)
                $("#genreTagContainer span." + rChildren[i]).addClass("collapse");
        }
    }

    //next, it's necessary to check the tags that have alternate parents to see if those parents are 
    //checked or they can be depopulated

    //set the value of the variable current to the first item in the list of children with a single dependency we might unload
    var current = mpndChildren[0];
    //set a variable to keep track of whether or not a parent is checked to false
    var parentChecked = false;
    //for each child in mpndChildren
    for (i = 0; i < mpndChildren.length; i++) {
        //because genre tags with their checkbox checked are only depopulated if they are no longer valid,
        //we must first check if any checked items are valid; as long as they are, we don't need to check
        //valid genre tags that are checked to see if they must be depopulated

        //if this child has its checkbox checked
        if ($("#genreTagContainer span." + mpndChildren[i] + " input").is(":checked")) {
            //if Fiction is currently selected
            if ($("#genreTagContainer span.1 input").is(":checked")) {
                //for each genre in nonfictionOnly
                for (j = 0; j < nonfictionOnly.length; j++) {
                    //if the current item in mpndChildren is the same as the current item in nonfictionOnly
                    if (mpndChildren[i] === nonfictionOnly[j]) {
                        //uncheck the checkbox because it is no longer valid
                        $("#genreTagContainer span." + mpndChildren[i] + " input").checked = false;

                        //if it isn't already collapsed, collapse it
                        if ($("#genreTagContainer span." + mpndChildren[i]).hasClass("collapse") !== true)
                            $("#genreTagContainer span." + mpndChildren[i]).addClass("collapse");

                        //Call toggle to let the input know to run its onchange function
                        $("#genreTagContainer span." + mpndChildren[i] + " input").toggle();
                    }
                }
            } //if Fiction is currently selected
            else { //if Nonfiction is currently selected
                //for each genre in fictionOnly
                for (j = 0; j < fictionOnly.length; j++) {
                    //if the current item in mpndChildren is the same as the current item in fictionOnly
                    if (mpndChildren[i] === fictionOnly[j]) {
                        //uncheck the checkbox because it is no longer valid
                        $("#genreTagContainer span." + mpndChildren[i] + " input").checked = false;

                        //if it isn't already collapsed, collapse it
                        if ($("#genreTagContainer span." + mpndChildren[i]).hasClass("collapse") !== true)
                            $("#genreTagContainer span." + mpndChildren[i]).addClass("collapse");

                        //Call toggle to let the input know to run its onchange function
                        $("#genreTagContainer span." + mpndChildren[i] + " input").toggle();
                    }
                }
            } //if Nonfiction is currently selected
        } //if this child has its checkbox checked
        else { //if this child doesn't have its checkbox checked
            //since some children appear in mpndChildren more than once due to having more than 
            //one possible alternate parent, we need to check if the for loop will continue to 
            //run after this and, if so, if the value of mpndChildren will be the same at the next
            //index; the reason for this is because we don't want to depopulate a child tag when
            //parentChecked is false unless it is still false after checking the last potential parent

            //if the next item in mpndChildren is the same as the current item
            if (i + 1 < mpndChildren.length && mpndChildren[i + 1] === current) {
                //if parentChecked is not already true and the current potential parent needs to be checked
                if (parentChecked !== true) {
                    //if this potential parent is checked, mark parentChecked true so that a check won't be made
                    //next iteration that might result in a false negative
                    if ($("#genreTagContainer span." + mpndParents[i] + " input").is(":checked"))
                        parentChecked = true;
                }
            } //if the next item in mpndChildren is the same as the current item
            else if (mpndChildren[i] === current) { //if mpndCurrent is the same as last time but will not be next time
                //basically this means this is the last chance to depopulate this child

                //if one of the possible parents for this child wasn't already checked on a previous iteration
                if (parentChecked !== true) {
                    //if the possible parent this time around isn't checked either
                    if ($("#genreTagContainer span." + mpndParents[i] + " input").is(":checked") !== true) {
                        //if the child isn't already collapsed, collapse it
                        if ($("#genreTagContainer span." + mpndChildren[i]).hasClass("collapse") !== true)
                            $("#genreTagContainer span." + mpndChildren[i]).addClass("collapse");
                    }
                }
            } //if mpndChildren is the same as last time but will not be next time
            else { //if this child isn't the same child as was being checked last iteration
                //reset the values of our variable to their default
                parentChecked = false;
                current = mpndChildren[i];

                //if this child has its matching parent checked, mark parentChecked as true
                if ($("#genreTagContainer span." + mpndParents[i] + " input").is(":checked")) {
                    parentChecked = true;
                } //if this potential parent isn't checked and mpndChildren will not be the same child next iteration
                else if (i + 1 === mpndChildren.length || mpndChildren[i + 1] !== current) {
                    //collapse this child if it isn't already collapsed
                    if ($("#genreTagContainer span." + mpndChildren[i]).hasClass("collapse") !== true)
                        $("#genreTagContainer span." + mpndChildren[i]).addClass("collapse");
                } //if this potential parent isn't checked and mpndChildren will not be the same child next iteration
            } //if this child isn't the same child as was being checked last iteration
        } //if this child doesn't have its checkbox checked
    } //for each child in mpndChildren

    //now we need to check the children with a single dependency and alternate possible parents, so reset the variables
    current = mpsdChildren[0];
    parentChecked = false;
    //for each child in mpsdChildren
    for (i = 0; i < mpsdChildren.length; i++) {
        //because genre tags with their checkbox checked are only depopulated if they are no longer valid,
        //we must first check if any checked items are valid; as long as they are, we don't need to check
        //valid genre tags that are checked to see if they must be depopulated

        //if this child has its checkbox checked
        if ($("#genreTagContainer span." + mpsdChildren[i] + " input").is(":checked")) {
            //if Fiction is currently selected
            if ($("#genreTagContainer span.1 input").is(":checked")) {
                //for each genre in nonfictionOnly
                for (j = 0; j < nonfictionOnly.length; j++) {
                    //if the current item in mpndChildren is the same as the current item in nonfictionOnly
                    if (mpsdChildren[i] === nonfictionOnly[j]) {
                        //uncheck the checkbox because it is no longer valid
                        $("#genreTagContainer span." + mpsdChildren[i] + " input").checked = false;

                        //if it isn't already collapsed, collapse it
                        if ($("#genreTagContainer span." + mpsdChildren[i]).hasClass("collapse") !== true)
                            $("#genreTagContainer span." + mpsdChildren[i]).addClass("collapse");

                        //Call toggle to let the input know to run its onchange function
                        $("#genreTagContainer span." + mpsdChildren[i] + " input").toggle();
                    }
                }
            } //if Fiction is currently selected
            else { //if Nonfiction is currently selected
                //for each genre in fictionOnly
                for (j = 0; j < fictionOnly.length; j++) {
                    //if the current item in mpsdChildren is the same as the current item in fictionOnly
                    if (mpsdChildren[i] === fictionOnly[j]) {
                        //uncheck the checkbox because it is no longer valid
                        $("#genreTagContainer span." + mpsdChildren[i] + " input").checked = false;

                        //if it isn't already collapsed, collapse it
                        if ($("#genreTagContainer span." + mpsdChildren[i]).hasClass("collapse") !== true)
                            $("#genreTagContainer span." + mpsdChildren[i]).addClass("collapse");

                        //Call toggle to let the input know to run its onchange function
                        $("#genreTagContainer span." + mpsdChildren[i] + " input").toggle();
                    }
                }
            } //if Nonfiction is currently selected
        } //if this child has its checkbox checked
        else { //if this child doesn't have its checkbox checked
            //since some children appear in mpsdChildren more than once due to having more than 
            //one possible alternate parent, we need to check if the for loop will continue to 
            //run after this and, if so, if the value of mpsdChildren will be the same at the next
            //index; the reason for this is because we don't want to depopulate a child tag when
            //parentChecked is false unless it is still false after checking the last potential parent

            //if the next item in mpsdChildren is the same as the current item
            if (i + 1 < mpsdChildren.length && mpsdChildren[i + 1] === current) {
                //if parentChecked is not already true and the current potential parent needs to be checked
                if (parentChecked !== true) {
                    //if this potential parent is checked, mark parentChecked true so that a check won't be made
                    //next iteration that might result in a false negative
                    if ($("#genreTagContainer span." + mpsdParents[i] + " input").is(":checked") && $("#genreTagContainer span." + mpsDependencies[i] + " input").is(":checked"))
                        parentChecked = true;
                }
            } //if the next item in mpsdChildren is the same as the current item
            else if (mpsdChildren[i] === current) { //if mpsdCurrent is the same as last time but will not be next time
                //basically this means this is the last chance to depopulate this child

                //if one of the possible parents for this child wasn't already checked on a previous iteration
                if (parentChecked !== true) {
                    //if the possible parent this time around isn't checked either
                    if ($("#genreTagContainer span." + mpndParents[i] + " input").is(":checked") !== true) {
                        //if the child isn't already collapsed, collapse it
                        if ($("#genreTagContainer span." + mpndChildren[i]).hasClass("collapse") !== true)
                            $("#genreTagContainer span." + mpndChildren[i]).addClass("collapse");
                    }
                }
            } //if mpsdChildren is the same as last time but will not be next time
            else { //if this child isn't the same child as was being checked last iteration
                //reset the values of our variable to their default
                parentChecked = false;
                current = mpsdChildren[i];

                //if this child has its matching parent checked, mark parentChecked as true
                if ($("#genreTagContainer span." + mpsdParents[i] + " input").is(":checked") && $("#genreTagContainer span." + mpsDependencies[i] + " input").is(":checked")) {
                    parentChecked = true;
                } //if this potential parent isn't checked and mpsdChildren will not be the same child next iteration
                else if (i + 1 === mpsdChildren.length || mpsdChildren[i + 1] !== current) {
                    //collapse this child if it isn't already collapsed
                    if ($("#genreTagContainer span." + mpsdChildren[i]).hasClass("collapse") !== true)
                        $("#genreTagContainer span." + mpsdChildren[i]).addClass("collapse");
                } //if this potential parent isn't checked and mpsdChildren will not be the same child next iteration
            } //if this child isn't the same child as was being checked last iteration
        } //if this child doesn't have its checkbox checked
    } //for each child in mpsdChildren
}