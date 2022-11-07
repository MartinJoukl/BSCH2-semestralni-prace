let typOsobyFunc = function () {
    let fyzO = document.querySelector("#fyzO");
    let pravO = document.querySelector("#pravO");
    let hiddenInput = document.querySelector('[name="zvolenyTypOsoby"]');

    //0 - fyzicka osoba, 1 - pravnicka osoba
    let types = document.getElementsByName("typOsoby");
    if (types[0].checked) {
        fyzO.classList.remove("noDisplay");
        pravO.classList.add("noDisplay");
        hiddenInput.setAttribute("value", "F");
    } else if (types[1].checked) {
        fyzO.classList.add("noDisplay");
        pravO.classList.remove("noDisplay");
        hiddenInput.setAttribute("value", "P");
    }
}

document.getElementsByName("typOsoby").forEach((elem) => {
    elem.addEventListener("change", typOsobyFunc);
});

console.log("JK");