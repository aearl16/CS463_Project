function showExplanation(explanation) {
    $("#formatTagDescription").empty();
    $("#formatTagDescription").append(explanation);
}

//sd stands for singular dependency and md stands for multiple dependency   
function ftChildren(id, sdChildren, mdChildren, dependencies) {
    if ($("#formatTagContainer span." + id + " input[type=checkbox]").is(':checked'))
        loadChildren(sdChildren, mdChildren, dependencies);
    else {
        for (var i = 0; i < mdChildren.length; i++)
            sdChildren.push(mdChildren[i]);

        unloadChildren(sdChildren);
    }
}

//sd stands for singular dependency and md stands for multiple dependency 
function loadChildren(sdChildren, mdChildren, dependencies) {
    for (var i = 0; i < sdChildren.length; i++) {

        if ($("#formatTagContainer span." + sdChildren[i]).hasClass("collapse"))
            $("#formatTagContainer span." + sdChildren[i]).removeClass("collapse");
    }

    for (i = 0; i < mdChildren.length; i++) {
        if ($("#formatTagContainer span." + dependencies[i] + " input[type=checkbox]").is(':checked')) {
            if ($("#formatTagContainer span." + dChildren[i]).hasClass("collapse"))
                $("#formatTagContainer span." + mdChildren[i]).removeClass("collapse");
        }
    }
}

function unloadChildren(children) {
    for (var i = 0; i < children.length; i++) {
        if ($("#formatTagContainer span." + children[i] + " input[type=checkbox]").is(':checked') !== true) {
            if ($("#formatTagContainer span." + children[i]).hasClass("collapse") !== true)
                $("#formatTagContainer span." + children[i]).addClass("collapse");
        }
    }
}