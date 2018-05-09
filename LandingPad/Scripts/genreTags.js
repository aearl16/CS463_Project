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
 * @param {int[]} parentFormats (formatIDs that aren't valid based on the item that was just selected)
 */
function fictionOrNonfiction(name, fictionOnly, nonfictionOnly, children, parentFormats) {
    console.log("Entering fictionOrNonfiction");
    console.log("name: " + name);
    console.log("fictionOnly: " + fictionOnly);
    console.log("nonfictionOnly: " + nonfictionOnly);
    console.log("children: " + children);
    console.log("parentFormats: " + parentFormats);

    //if this is the first time either fiction or nonfiction is being selected, 
    //remove the collapse class from the rest of the genre tags
    if ($(".genreTagsThreePlus").hasClass("collapse")) {
        console.log("Uncollapsing genreTagsThreePlus");
        $(".genreTagsThreePlus").removeClass("collapse");
    }

    //for each of this genre's children
    for (var i = 0; i < children.length; i++) {
        console.log("Checking child " + children[i]);
        //if the child is collapsed, uncollapse it
        if ($("#genreTagContainer span." + children[i]).hasClass("collapse")) {
            console.log("Uncollapsing child " + children[i]);
            $("#genreTagContainer span." + children[i]).removeClass("collapse");
        }
    }

    //if fiction is the one that was just selected
    if (name === "Fiction") {
        console.log("Entering Fiction to check for currently visible items that are nonfiction only.");
        //for each genre tag that is nonfiction only
        for (i = 0; i < nonfictionOnly.length; i++) {
            console.log("Checking nonfiction only genre " + nonfictionOnly[i]);
            //if it isn't currently collapsed
            if ($("#genreTagContainer span." + nonfictionOnly[i]).hasClass("collapse") !== true) {
                console.log("Genre is currently not collapsed.");
                //if it is currently checked, uncheck it
                if ($("#genreTagContainer span." + nonfictionOnly[i] + " input").is(':checked')) {
                    console.log("Genre is currently checked; unchecking genre.");
                    $("#genreTagContainer span." + nonfictionOnly[i] + " input").prop(':checked', false);
                } 

                //collapse it
                console.log("Collapsing genre and triggering onchange for genre " + nonfictionOnly[i]);
                $("#genreTagContainer span." + nonfictionOnly[i]).addClass("collapse");

                //and let the checkbox know that it has been changed so the fuction on it will run
                $("#genreTagContainer span." + nonfictionOnly[i] + " input").change();
            } //if it isn't currently collapsed
        } //for each genre tag that is nonfiction only
    } //if fiction is the one that was just selected
    else if (name === "Nonfiction") { //if nonfiction is the one that was just selected
        console.log("Entering Nonfiction to check for currently visible items that are fiction only.");
        //for each genre tag that is fiction only
        for (i = 0; i < fictionOnly.length; i++) {
            console.log("Checking fiction only genre " + fictionOnly[i]);
            //if it isn't already collapsed
            if ($("#genreTagContainer span." + fictionOnly[i]).hasClass("collapse") !== true) {
                console.log("Genre is not currently collapsed.");
                //if it is currently checked, uncheck it
                if ($("#genreTagContainer span." + fictionOnly[i] + " input").is(':checked')) {
                    console.log("Genre is currently checked. Unchecking genre.");
                    $("#genreTagContainer span." + fictionOnly[i] + " input").prop(':checked', false);
                }

                console.log("Collapsing genre and triggering onchange for genre " + fictionOnly[i]);
                //collapse it
                $("#genreTagContainer span." + fictionOnly[i]).addClass("collapse");

                //and let the checkbox know that it has been changed so the fuction on it will run
                $("#genreTagContainer span." + fictionOnly[i] + " input").change();
            } //if it isn't currently collapsed
        } //for each genre tag that is fiction only
    } //if nonfiction is the one that was just selected
    else {
        console.log("Entering no selection.");
    }

    //for each format in parentFormats
    for (i = 0; i < parentFormats.length; i++) {
        //if the current format tag is checked, uncheck it and then trigger onchange so the functions will run
        if ($("#formatTagContainer span." + parentFormats[i] + " input[type=checkbox]").is(':checked')) {
            console.log("Unchecking format tag " + parentFormats[i] + " with opposite genre type and triggering onchange.");
            $("#formatTagContainer span." + parentFormats[i] + " input[type=checkbox]").prop(':checked', false);
            $("#formatTagContainer span." + parentFormats[i] + " input[type=checkbox]").change();
        }
    } //for each format in parentFormats
    console.log("Exiting fictionOrNonfiction.");
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
 * @param {int[]} parentFormats (array of formats that are parents of the current tag)
 * @param {int[]} parentNames (the actual names for the format tags that are parents)
 */
function gtChildren(id, ndChildren, sdChildren, sDependencies, mdChildren, mdfDependencies, mdsDependencies, mpndChildren, mpndParents, mpsdChildren, mpsdParents, mpsDependencies, fictionOnly, nonfictionOnly, parentFormats, parentNames) {
    console.log("Entering gtChildren for id " + id);
    console.log("ndChildren: " + ndChildren);
    console.log("sdChildren: " + sdChildren);
    console.log("sDependencies: " + sDependencies);
    console.log("mdChildren: " + mdChildren);
    console.log("mdfDependencies: " + mdfDependencies);
    console.log("mdsDependencies: " + mdsDependencies);
    console.log("mpndChildren: " + mpndChildren);
    console.log("mpndParents: " + mpndParents);
    console.log("mpsdChildren: " + mpsdChildren);
    console.log("mpsdParents: " + mpsdParents);
    console.log("mpsDependencies: " + mpsDependencies);
    console.log("fictionOnly: " + fictionOnly);
    console.log("nonfictionOnly: " + nonfictionOnly);
    console.log("parentFormats: " + parentFormats);
    console.log("parentNames: " + parentNames);

    //if the checkbox for the genre tag with a GenreID of id was checked
    if ($("#genreTagContainer span." + id + " input").is(':checked')) {
        console.log("Genre with id of " + id + " is currently checked, checking for required parents before loading children.");
        //if the current genre tag has required parent formats, you need to check to make sure that one of them is checked before loading children
        if (parentFormats.length > 0) {
            console.log("Required parents found. There are " + parentFormats.length + " potential parent formats.");
            console.log("Setting parentsChecked to false. Checking to make sure selection is valid.");
            var parentChecked = false;
            //for each parent format
            for (var i = 0; i < parentFormats.length; i++) {
                console.log("Checking to see if format with id of " + parentFormats[i] + " is checked");
                //if the current parentFormat is checked, mark it as true and exit so that it doesn't continue to run
                if ($("#formatTagContainer span." + parentFormats[i] + " input[type=checkbox]").is(':checked')) {
                    console.log("Parent format is checked. Setting parentChecked to true and exiting for loop.");
                    parentChecked = true;
                    break;
                }
                console.log("Value of parentChecked is currently " + parentChecked);
            }
            console.log("parentFormats for loop exited, checking value of parentChecked.");

            //if parentChecked is still false
            if (parentChecked === false) {
                console.log("None of the parentFormats are selected. Creating error message.");
                var message = "This genre requires you to select one of the following formats:&quot;br /&quot;";

                //for each of the format tags that you can select
                for (i = 0; i < parentNames.length; i++) {
                    message += parentNames[i];
                    message += "&quot;br /&quot;";
                    console.log("message: " + message);
                }

                console.log("Displaying error message and unchecking genre with id of " + id);
                //call a function to display the error message that we created
                showExplanationNotFT("genreTagDescription", "&quot;" + message + "&quot;");
                //then uncheck the item
                $("#genreTagContainer span." + id + " input").prop(':checked', false);
            } //if none of the parent formats are checked
            else { //if this is a valid selection, load the child genres
                console.log("Selection is valid. Calling loadGenres with values");
                console.log("ndChildren: " + ndChildren);
                console.log("sdChildren: " + sdChildren);
                console.log("sDependencies: " + sDependencies);
                console.log("mdChildren: " + mdChildren);
                console.log("mdfDependencies: " + mdfDependencies);
                console.log("mdsDependencies: " + mdsDependencies);
                //loadGenres(ndChildren, sdChildren, sDependencies, mdChildren, mdfDependencies, mdsDependencies);
            } //if this is a valid selection, load the child genres
        } //if the current genre tag has required parent formats 
        else { //if there are no required formats, you can just call load
            console.log("No require parents found. Calling loadGenres with values");
            console.log("ndChildren: " + ndChildren);
            console.log("sdChildren: " + sdChildren);
            console.log("sDependencies: " + sDependencies);
            console.log("mdChildren: " + mdChildren);
            console.log("mdfDependencies: " + mdfDependencies);
            console.log("mdsDependencies: " + mdsDependencies);
            //load all no dependency children and load any single dependency children with their other dependency checked and 
            //any multi-dependency children with their other two dependencies checked
            //loadGenres(ndChildren, sdChildren, sDependencies, mdChildren, mdfDependencies, mdsDependencies);
        } //if there are no required formats, you can just call load
    } //if the checkbox for the genre tag with a GenreID of id was checked
    else { //if we are unloading children, we only care about dependencies of mpsdChildren
        console.log("Genre of id " + id + " is not checked. Getting ready to unload children.");
        console.log("Checking ndChildren for alternate possible parents; ndChildren is currently: " + ndChildren);
        //for each no dependency child
        for (i = 0; i < ndChildren.length; i++) {
            console.log("Checking ndChild with id of " + ndChildren[i] + " for alternate possible parents.");
            //for each no dependency child where the child has more than one possible parent
            for (var j = 0; j < mpndChildren.length; j++) {
                console.log("Checking for match between ndChild " + ndChildren[i] + " and mpndChild " + mpndChildren[j]);
                //if the no dependency child and the child with more than one possible parent are the same
                if (ndChildren[i] === mpndChildren[j]) {
                    console.log("Match found. Removing ndChild " + i);
                    //remove the no dependency child from the array
                    ndChildren.splice(i, 1);
                    console.log("ndChildren is now " + ndChildren);
                } //if the no dependency child and the child with more than one possible parent are the same
            } //for each no dependency child where the child has more than one possible parent
        } //for each no dependency child

        console.log("Checking sdChildren for alternate possible parents; sdChildren is currently: " + sdChildren);
        //for each single dependency child
        for (i = 0; i < sdChildren.length; i++) {
            console.log("Checking sdChild with id of " + sdChildren[i] + " for alternate possible parents.");
            //for each single dependency child where the child has more than one possible parent
            for (j = 0; j < mpsdChildren.length; j++) {
                console.log("Checking sdChild " + sdChildren[i] + " and mpsdChild " + mpsdChildren[j] + " for a match.");
                //if the single dependency child and the child with more than one possible parent are the same
                if (sdChildren[i] === mpsdChildren[j]) {
                    console.log("Match found. Removing sdChild " + sdChildren[i] + " from sdChildren.");
                    //remove the single dependency child from the array
                    sdChildren.splice(i, 1);
                    console.log("sdChildren is now: " + sdChildren);
                }
            }
        }

        console.log("Aggregating results of sdChildren to ndChildren. ndChildren is currently: " + ndChildren);
        //add all the single dependency children to the same array as the no dependency children
        for (i = 0; i < sdChildren.length; i++) {
            console.log("Adding sdChild " + sdChildren[i] + " to ndChildren.");
            ndChildren.push(sdChildren[i]);
        }
        console.log("After adding " + sdChildren.length + " sdChildren to ndChildren, ndChildren is now: " + ndChildren);  

        console.log("Aggregating results of mdChildren to ndChildren.");
        //add all the multi-dependency children to the same array as the no dependency children
        for (i = 0; i < mdChildren.length; i++) {
            console.log("Adding mdChild " + mdChildren[i] + " to ndChildren.");
            ndChildren.push(mdChildren[i]);
        }
        console.log("After adding " + mdChildren.length + " mdChildren to ndChildren, ndChildren is now: " + ndChildren);

        console.log("Calling unloadGenres for values");
        console.log("ndChildren: " + ndChildren);
        console.log("mpndChildren: " + mpndChildren);
        console.log("mpndParents: " + mpndParents);
        console.log("mpsdChildren: " + mpsdChildren);
        console.log("mpsdParents: " + mpsdParents);
        console.log("mpsDependencies: " + mpsDependencies);
        console.log("fictionOnly: " + fictionOnly);
        console.log("nonfictionOnly: " + nonfictionOnly);
        //call the function to unload all of the no dependency children and single dependency
        //and multi-dependency children with only one possible combination of parents; remove
        //the children with multiple possible parents or combination of parents only if none of
        //the parents or combination of parents are selected
        //unloadGenres(ndChildren, mpndChildren, mpndParents, mpsdChildren, mpsdParents, mpsDependencies, fictionOnly, nonfictionOnly);
    }
}

function loadGenres(ndChildren, sdChildren, sDependencies, mdChildren, mdfDependencies, mdsDependencies) {
    console.log("Entering loadGenres.");
    //for each child without any dependencies
    for (var i = 0; i < ndChildren.length; i++) {
        //if the child is currently collapsed, uncollapse it
        if ($("#genreTagContainer span." + ndChildren[i]).hasClass("collapse"))
            $("#genreTagContainer span." + ndChildren[i]).removeClass("collapse");
    }

    //for each child with only one dependency
    for (i = 0; i < sdChildren.length; i++) {
        //if the dependency for this child is checked
        if ($("#genreTagContainer span." + sDependencies[i] + " input").is(':checked')) {
            //if the child is currently collapsed, uncollapse it
            if ($("#genreTagContainer span." + sdChildren[i]).hasClass("collapse"))
                $("#genreTagContainer span." + sdChildren[i]).removeClass("collapse");
        }
    }

    //for each child with two dependencies
    for (i = 0; i < mdChildren.length; i++) {
        //if both dependencies for this child are checked 
        if ($("#genreTagContainer span." + mdfDependencies[i] + " input").is(':checked') && $("#genreTagContainer span." + mdsDependencies[i] + " input").is(':checked')) {
            //if the child is currently collapsed, uncollapse it
            if ($("#genreTagContainer span." + mdChildren[i]).hasClass("collapse"))
                $("#genreTagContainer span." + mdChildren[i]).removeClass("collapse");
        }
    }
}

function unloadGenres(rChildren, mpndChildren, mpndParents, mpsdChildren, mpsdParents, mpsDependencies, fictionOnly, nonfictionOnly) {
    console.log("Entering unloadGenres.");
    //for all of the children that we KNOW can be depopulated
    for (var i = 0; i < rChildren.length; i++) {
        //if the checkbox for this child is checked
        if ($("#genreTagContainer span." + rChildren[i] + " input").is(':checked')) {
            //if Fiction is currently selected
            if ($("#genreTagContainer span.1 input").is(':checked')) {
                //for each genre in nonfictionOnly
                for (var j = 0; j < nonfictionOnly.length; j++) {
                    //if the current item in rChildren is the same as the current item in nonfictionOnly
                    if (rChildren[i] === nonfictionOnly[j]) {
                        //uncheck the checkbox because it is no longer valid
                        $("#genreTagContainer span." + rChildren[i] + " input").prop(':checked', false);

                        //if it isn't already collapsed, collapse it
                        if ($("#genreTagContainer span." + rChildren[i]).hasClass("collapse") !== true)
                            $("#genreTagContainer span." + rChildren[i]).addClass("collapse");

                        //Trigger onchange
                        $("#genreTagContainer span." + rChildren[i] + " input").change();
                    }
                }
            }
            else { //if Nonfiction is currently selected
                //for each genre in fictionOnly
                for (j = 0; j < fictionOnly.length; j++) {
                    //if the current item in rChildren is the same as the current item in fictionOnly
                    if (rChildren[i] === fictionOnly[j]) {
                        //uncheck the checkbox because it is no longer valid
                        $("#genreTagContainer span." + rChildren[i] + " input").prop(':checked', false);

                        //if it isn't already collapsed, collapse it
                        if ($("#genreTagContainer span." + rChildren[i]).hasClass("collapse") !== true)
                            $("#genreTagContainer span." + rChildren[i]).addClass("collapse");

                        //Trigger onchange
                        $("#genreTagContainer span." + rChildren[i] + " input").change();
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
        if ($("#genreTagContainer span." + mpndChildren[i] + " input").is(':checked')) {
            //if Fiction is currently selected
            if ($("#genreTagContainer span.1 input").is(':checked')) {
                //for each genre in nonfictionOnly
                for (j = 0; j < nonfictionOnly.length; j++) {
                    //if the current item in mpndChildren is the same as the current item in nonfictionOnly
                    if (mpndChildren[i] === nonfictionOnly[j]) {
                        //uncheck the checkbox because it is no longer valid
                        $("#genreTagContainer span." + mpndChildren[i] + " input").prop(':checked', false);

                        //if it isn't already collapsed, collapse it
                        if ($("#genreTagContainer span." + mpndChildren[i]).hasClass("collapse") !== true)
                            $("#genreTagContainer span." + mpndChildren[i]).addClass("collapse");

                        //Trigger onchange
                        $("#genreTagContainer span." + mpndChildren[i] + " input").change();
                    }
                }
            } //if Fiction is currently selected
            else { //if Nonfiction is currently selected
                //for each genre in fictionOnly
                for (j = 0; j < fictionOnly.length; j++) {
                    //if the current item in mpndChildren is the same as the current item in fictionOnly
                    if (mpndChildren[i] === fictionOnly[j]) {
                        //uncheck the checkbox because it is no longer valid
                        $("#genreTagContainer span." + mpndChildren[i] + " input").prop(':checked', false);

                        //if it isn't already collapsed, collapse it
                        if ($("#genreTagContainer span." + mpndChildren[i]).hasClass("collapse") !== true)
                            $("#genreTagContainer span." + mpndChildren[i]).addClass("collapse");

                        //Trigger onchange
                        $("#genreTagContainer span." + mpndChildren[i] + " input").change();
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
                    if ($("#genreTagContainer span." + mpndParents[i] + " input").is(':checked'))
                        parentChecked = true;
                }
            } //if the next item in mpndChildren is the same as the current item
            else if (mpndChildren[i] === current) { //if mpndCurrent is the same as last time but will not be next time
                //basically this means this is the last chance to depopulate this child

                //if one of the possible parents for this child wasn't already checked on a previous iteration
                if (parentChecked !== true) {
                    //if the possible parent this time around isn't checked either
                    if ($("#genreTagContainer span." + mpndParents[i] + " input").is(':checked') !== true) {
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
                if ($("#genreTagContainer span." + mpndParents[i] + " input").is(':checked')) {
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
        if ($("#genreTagContainer span." + mpsdChildren[i] + " input").is(':checked')) {
            //if Fiction is currently selected
            if ($("#genreTagContainer span.1 input").is(':checked')) {
                //for each genre in nonfictionOnly
                for (j = 0; j < nonfictionOnly.length; j++) {
                    //if the current item in mpndChildren is the same as the current item in nonfictionOnly
                    if (mpsdChildren[i] === nonfictionOnly[j]) {
                        //uncheck the checkbox because it is no longer valid
                        $("#genreTagContainer span." + mpsdChildren[i] + " input").prop(':checked', false);

                        //if it isn't already collapsed, collapse it
                        if ($("#genreTagContainer span." + mpsdChildren[i]).hasClass("collapse") !== true)
                            $("#genreTagContainer span." + mpsdChildren[i]).addClass("collapse");

                        //Trigger onchange
                        $("#genreTagContainer span." + mpsdChildren[i] + " input").change();
                    }
                }
            } //if Fiction is currently selected
            else { //if Nonfiction is currently selected
                //for each genre in fictionOnly
                for (j = 0; j < fictionOnly.length; j++) {
                    //if the current item in mpsdChildren is the same as the current item in fictionOnly
                    if (mpsdChildren[i] === fictionOnly[j]) {
                        //uncheck the checkbox because it is no longer valid
                        $("#genreTagContainer span." + mpsdChildren[i] + " input").prop(':checked', false);

                        //if it isn't already collapsed, collapse it
                        if ($("#genreTagContainer span." + mpsdChildren[i]).hasClass("collapse") !== true)
                            $("#genreTagContainer span." + mpsdChildren[i]).addClass("collapse");

                        //Trigger onchange
                        $("#genreTagContainer span." + mpsdChildren[i] + " input").change();
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
                    if ($("#genreTagContainer span." + mpsdParents[i] + " input").is(':checked') && $("#genreTagContainer span." + mpsDependencies[i] + " input").is(':checked'))
                        parentChecked = true;
                }
            } //if the next item in mpsdChildren is the same as the current item
            else if (mpsdChildren[i] === current) { //if mpsdCurrent is the same as last time but will not be next time
                //basically this means this is the last chance to depopulate this child

                //if one of the possible parents for this child wasn't already checked on a previous iteration
                if (parentChecked !== true) {
                    //if the possible parent this time around isn't checked either
                    if ($("#genreTagContainer span." + mpndParents[i] + " input").is(':checked') !== true) {
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
                if ($("#genreTagContainer span." + mpsdParents[i] + " input").is(':checked') && $("#genreTagContainer span." + mpsDependencies[i] + " input").is(':checked')) {
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