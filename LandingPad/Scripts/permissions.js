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

function changeAccess(id) {
    if (id === "public" && $("span." + id + " input[type=checkbox]").is(':checked')) {
        $("span.friend input[type=checkbox]").prop('checked', true);
        $("span.publisher input[type=checkbox]").prop('checked', true);
    }
    else if ((id === "friend" || id === "publisher") && $("span." + id + " input[type=checkbox]").is(':checked') !== true) {
        $("span.public input[type=checkbox]").prop('checked', false);
    }
}

function loadPseudonyms(profiles) {
    var id = $("#profileID").val();
    var grants = $(".grant");
    var revokes = $(".revoke");

    $("#pseudonymContainer input").prop('checked', false);

    for (var i = 0; i < profiles.length; i++) {
        if ($("#pseudonymContainer > span." + profiles[i]).hasClass("collapse") !== true)
            $("#pseudonymContainer > span." + profiles[i]).addClass("collapse");
    }

    $("#pseudonymContainer span." + id).removeClass("collapse");
}

function checkIfOnlyPseudonym() {
    var id = $("#profileID").val();

    if ($(this).prop('checked', true)) {
        if ($("#useUsername").hasClass("collapse"))
            $("#useUsername").removeClass("collapse");
    }
    else {
        var pseudonyms = $("#pseudonymContainer > span." + id + " input");

        for (var i = 0; i < pseudonyms.length; i++) {
            if (pseudonyms[i].prop('checked', true)) 
                return;
        }

        if ($("#useUsername").hasClass("collapse") !== true)
            $("#useUsername").addClass("collapse");

        $("#usePseudonymsInAdditionToUsername").prop('checked', false);
    }
}