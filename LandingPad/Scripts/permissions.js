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

    $("#pseudonymContainer input").prop('checked', false);

    for (var i = 0; i < profiles.length; i++) {
        if ($("#pseudonymContainer > span." + profiles[i]).hasClass("collapse") !== true)
            $("#pseudonymContainer > span." + profiles[i]).addClass("collapse");
    }

    $("#pseudonymContainer span." + id).removeClass("collapse");
}