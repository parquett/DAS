function getTranslation(toTranslate) {
    var translatedValue = "";
    translatedValue = Translations[gCurrentLanguage][toTranslate];
    if (typeof translatedValue == "undefined")
        translatedValue = toTranslate;
    return translatedValue;
}

var Translations = {
    1: { /*English*/
    },
    2: { /*Romanian*/
        ["Save"]: ["Salveaza"],
        ["Back"]: ["Inapoi"],
        ["Close"]: ["Inchide"],
        ["Cancel"]: ["Anulare"],

    }
}