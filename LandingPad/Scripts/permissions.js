/**
 * Scripts for dealing with the permissions on the _Menu partial view
 * and all other partial views derived from it; also has some scripts
 * for handling pseudonym selection on the same partial views
 */

/**
 * A function for emptying a field and appending an explanation to it.
 * Unlike the similar function in formatTags.js, it lets you specify
 * the container you are emptying/appending
 * @param {string} id (the id of the container you want to empty/write to as a string)
 * @param {string} explanation (explanation of whatever from the partial view)
 */
function showExplanationNotFT(id, explanation) {
    $("#" + id).empty();
    $("#" + id).append(explanation);
}

/**
 * Function to make sure that publisher and friend access are always selected if public
 * access is and that public access isn't checked if both publisher and friend access
 * aren't
 *
 * @param {string} id (the class name of the container for the checkbox you are changing)
 */
function changeAccess(id) {
    //if public access is checked, check friend access and publisher access
    if (id === "public" && $("span." + id + " input[type=checkbox]").is(':checked')) {
        $("span.friend input[type=checkbox]").prop('checked', true);
        $("span.publisher input[type=checkbox]").prop('checked', true);
    } //if friend access or publisher access were unchecked, uncheck public access as well
    else if ((id === "friend" || id === "publisher") && $("span." + id + " input[type=checkbox]").is(':checked') !== true) {
        $("span.public input[type=checkbox]").prop('checked', false);
    }
}

/**
 * A function to bring up the correct pseudonyms for the selected ProfileID
 * @param {int[]} profiles (ProfileIDs from the partial view)
 */
function loadPseudonyms(profiles) {
    //get the current ProfileID
    var id = $("#profileID").val();

    //uncheck all of the checked pseudonyms when the ProfileID is changed so that we don't
    //end up with pseudonyms that don't belong to the writing's writer being associated with
    //the writing
    $("#pseudonymContainer input").prop('checked', false);

    //for each ProfileID
    for (var i = 0; i < profiles.length; i++) {
        //if the pseudonyms for that profile are not currently collapsed, collapse them
        if ($("#pseudonymContainer > span." + profiles[i]).hasClass("collapse") !== true)
            $("#pseudonymContainer > span." + profiles[i]).addClass("collapse");
    }

    //uncollapse the pseudonyms for the selected profile
    $("#pseudonymContainer span." + id).removeClass("collapse");

    //collapse useUsername if it isn't already selected
    if ($("#useUsername").hasClass("collapse") !== true)
        $("#useUsername").addClass("collapse");

    //make sure the checkbox for using pseudonyms in addition to username is unchecked
    //so we don't get a false positive on submission
    $("#usePseudonymsInAdditionToUsername").prop('checked', false);
} //loadPseudonyms(profiles)

/**
 * A function to bring up the checkbox for indicating whether or not
 * psudonyms should be used in addition to username if there is at least
 * one pseudonym selected; removes the checkbox and unchecks it if a
 * pseudonym is unchecked when no others are already checked
 */
function checkIfOnlyPseudonym() {
    //get the current ProfileID
    var id = $("#profileID").val();

    //if the function is being called because a pseudonym was checked
    if ($(this).prop('checked', true)) {
        //if useUsername is collapsed, uncollapse it
        if ($("#useUsername").hasClass("collapse"))
            $("#useUsername").removeClass("collapse");
    }
    else { //if it was called by unchecking
        //grab all of the pseudonyms related to this profile
        var pseudonyms = $("#pseudonymContainer > span." + id + " input");

        //for each of the pseudonym checkboxes for this profile
        for (var i = 0; i < pseudonyms.length; i++) {
            //if at least one of them is checked, nothing more needs to be done, so return
            if (pseudonyms[i].prop('checked', true)) 
                return;
        }

        //if the end of the for loop has been reached without finding a checked pseudonym,
        //collapse useUsername if it isn't already selected
        if ($("#useUsername").hasClass("collapse") !== true)
            $("#useUsername").addClass("collapse");

        //make sure the checkbox for using pseudonyms in addition to username is unchecked
        //so we don't get a false positive on submission
        $("#usePseudonymsInAdditionToUsername").prop('checked', false);
    }
} //checkIfOnlyPseudonym()