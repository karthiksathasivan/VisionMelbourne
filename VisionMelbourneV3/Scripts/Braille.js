var pattern = "";
var text = "";
var i = 0;
function doTTS() {
    switch (true) {
        case pattern.includes(1) && i === 1:
            text = "A";
            break;
        case pattern.includes(1) && pattern.includes(2) && i === 2:
            text = "B";
            break;
        case pattern.includes(1) && pattern.includes(4) && i === 2:
            text = "C";
            break;
        case pattern.includes(1) && pattern.includes(4) &&
            pattern.includes(5) && i === 3:
            text = "D";
            break;
        case pattern.includes(1) && pattern.includes(5) && i === 2:
            text = "E";
            break;
        case pattern.includes(1) && pattern.includes(2) &&
            pattern.includes(4) && i === 3:
            text = "F";
            break;
        case pattern.includes(1) && pattern.includes(2) &&
            pattern.includes(4) && pattern.includes(5) && i === 4:
            text = "G";
            break;
        case pattern.includes(1) && pattern.includes(2) &&
            pattern.includes(5) && i === 3:
            text = "H";
            break;
        case pattern.includes(2) && pattern.includes(4) && i === 2:
            text = "I";
            break;
        case pattern.includes(2) && pattern.includes(4) &&
            pattern.includes(5) && i === 3:
            text = "J";
            break;
        case pattern.includes(1) && pattern.includes(3) && i === 2:
            text = "K";
            break;
        case pattern.includes(1) && pattern.includes(2) &&
            pattern.includes(3) && i === 3:
            text = "L";
            break;
        case pattern.includes(1) && pattern.includes(4) &&
            pattern.includes(3) && i === 3:
            text = "M";
            break;
        case pattern.includes(1) && pattern.includes(3) &&
            pattern.includes(4) && pattern.includes(5) && i === 4:
            text = "N";
            break;
        case pattern.includes(1) && pattern.includes(3) &&
            pattern.includes(5) && i === 3:
            text = "O";
            break;
        case pattern.includes(1) && pattern.includes(2) &&
            pattern.includes(3) && pattern.includes(4) && i === 4:
            text = "P";
            break;
        case pattern.includes(1) && pattern.includes(2) &&
            pattern.includes(3) && pattern.includes(4) && pattern.includes(5) && i === 5:
            text = "Q";
            break;
        case pattern.includes(1) && pattern.includes(2) &&
            pattern.includes(3) && pattern.includes(5) && i === 4:
            text = "R";
            break;
        case pattern.includes(2) && pattern.includes(3) &&
            pattern.includes(4) && i === 3:
            text = "S";
            break;
        case pattern.includes(2) && pattern.includes(3) &&
            pattern.includes(4) && pattern.includes(5) && i === 4:
            text = "T";
            break;
        case pattern.includes(1) && pattern.includes(3) &&
            pattern.includes(6) && i === 3:
            text = "U";
            break;
        case pattern.includes(1) && pattern.includes(2) &&
            pattern.includes(3) && pattern.includes(6) && i === 4:
            text = "V";
            break;
        case pattern.includes(2) && pattern.includes(6) &&
            pattern.includes(4) && pattern.includes(5) && i === 4:
            text = "W";
            break;
        case pattern.includes(1) && pattern.includes(3) &&
            pattern.includes(4) && pattern.includes(6) && i === 4:
            text = "X";
            break;
        case pattern.includes(1) && pattern.includes(3) &&
            pattern.includes(4) && pattern.includes(5) && pattern.includes(6) && i === 5:
            text = "Y";
            break;
        case pattern.includes(1) && pattern.includes(3) &&
            pattern.includes(6) && pattern.includes(5) && i === 4:
            text = "Z";
            break;
        default:
            text = "Braille code not found";
    }
    var displaytext = document.getElementById('Text');
    displaytext.innerHTML = "<h1>"+text+"<h1>";
    const utterance = new SpeechSynthesisUtterance(text);
    utterance.pitch = 1.5;
    utterance.volume = 1.5;
    utterance.rate = 1;
    speechSynthesis.speak(utterance);
    pattern = "";
    text = "";
    i = 0;
}


function clickFunction(element) {
    var code = document.getElementById(element.id).value;
    pattern += code;
    i++;
}

var location = document.getElementById('brailleimage');
var img = [];
var brailleimageurl = "";

function findBraille() {

    var letter = "";
    var input = document.getElementById("brailletext").value;
    for (j = 0; j < input.length; j++)
    {
        letter = input.charAt(j); 
        displayBraille(letter.toLowerCase());
    }
}

//function displayBraille(pattern) {
//    switch (true) {
//        case pattern.includes("a"):
//            text = '<img src="~/images/icon01.png" />';
//            break;
//        case pattern.includes("b"):
//            text = '<img src="~/images/icon01.png" />';
//            break;
//        case pattern.includes("c"):
//            text = '<img src="~/images/icon01.png" />';
//            break;
//        case pattern.includes("d") :
//            text = "D";
//            break;
//        case pattern.includes("e") :
//            text = "E";
//            break;
//        case pattern.includes("f"):
//            text = "F";
//            break;
//        case pattern.includes("g"):
//            text = "G";
//            break;
//        case pattern.includes("h"):
//            text = "H";
//            break;
//        case pattern.includes("i"):
//            text = "I";
//            break;
//        case pattern.includes("j"):
//            text = "J";
//            break;
//        case pattern.includes("k"):
//            text = "K";
//            break;
//        case pattern.includes("l"):
//            text = "L";
//            break;
//        case pattern.includes("m"):
//            text = "M";
//            break;
//        case pattern.includes("n"):
//            text = "N";
//            break;
//        case pattern.includes("o"):
//            text = "O";
//            break;
//        case pattern.includes("p"):
//            text = "P";
//            break;
//        case pattern.includes("q"):
//            text = "Q";
//            break;
//        case pattern.includes("r"):
//            text = "R";
//            break;
//        case pattern.includes("s"):
//            text = "S";
//            break;
//        case pattern.includes("t"):
//            text = "T";
//            break;
//        case pattern.includes("u"):
//            text = "U";
//            break;
//        case pattern.includes("v"):
//            text = "V";
//            break;
//        case pattern.includes("w"):
//            text = "W";
//            break;
//        case pattern.includes("x"):
//            text = "X";
//            break;
//        case pattern.includes("y"):
//            text = "Y";
//            break;
//        case pattern.includes("z"):
//            text = "Z";
//            break;
//        default:
//            text = "Braille code not found";
//    }
//    return text;
//}
