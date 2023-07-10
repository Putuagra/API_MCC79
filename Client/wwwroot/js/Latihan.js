let btn1 = document.getElementById("btn1");
let btn2 = document.getElementById("btn2");
let btn3 = document.getElementById("btn3");
let btn4 = document.getElementById("btn4");
let p1 = document.getElementById("satu");
let p2 = document.getElementById("dua");
let p3 = document.getElementById("tiga");

function random(number) {
    return Math.floor(Math.random() * (number + 1))
}

btn1.addEventListener("click", () => {
    let rndCol = `rgb(${random(255)}, ${random(255)}, ${random(255)})`;
    p1.style.backgroundColor = rndCol;
})

btn2.addEventListener("click", () => {
    let rndCol = `rgb(${random(255)}, ${random(255)}, ${random(255)})`;
    p2.style.backgroundColor = rndCol;
})

btn3.addEventListener("click", () => {
    let rndCol = `rgb(${random(255)}, ${random(255)}, ${random(255)})`;
    p3.style.backgroundColor = rndCol;
})

btn4.addEventListener("click", () => {
    p1.style.backgroundColor = "transparent";
    p2.style.backgroundColor = "transparent";
    p3.style.backgroundColor = "transparent";
})

let arrayMhsObj = [
    { nama: "budi", nim: "a112015", umur: 20, isActive: true, fakultas: { name: "komputer" } },
    { nama: "joko", nim: "a112035", umur: 22, isActive: false, fakultas: { name: "ekonomi" } },
    { nama: "herul", nim: "a112020", umur: 21, isActive: true, fakultas: { name: "komputer" } },
    { nama: "herul", nim: "a112032", umur: 25, isActive: true, fakultas: { name: "ekonomi" } },
    { nama: "herul", nim: "a112040", umur: 21, isActive: true, fakultas: { name: "komputer" } },
]

let fakultasKomputer = []
for (var i = 0; i < arrayMhsObj.length; i++) {
    if (arrayMhsObj[i].fakultas.name === 'komputer') {
        fakultasKomputer.push(arrayMhsObj[i])
    }
}
console.log(fakultasKomputer)

for (var i = 0; i < arrayMhsObj.length; i++) {
    if (arrayMhsObj[i].nim.substring(5) >= '30') {
        arrayMhsObj[i].isActive = false
    } else {
        arrayMhsObj[i].isActive = true
    }
}
console.log(arrayMhsObj)