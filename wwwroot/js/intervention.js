document.addEventListener("DOMContentLoaded", function () {
    // Gestion de la visibilité des pièces et des quantités
    const checkboxNecessitePieces = document.getElementById("NecessitePieces");
    const piecesContainer = document.getElementById("piecesContainer");

    // Affiche/Masque les pièces en fonction de la case "NecessitePieces"
    piecesContainer.style.display = checkboxNecessitePieces.checked ? "block" : "none";

    checkboxNecessitePieces.addEventListener("change", function () {
        piecesContainer.style.display = checkboxNecessitePieces.checked ? "block" : "none";
    });

    // Gérer l'affichage de la quantité lorsque la case d'une pièce est cochée/décochée
    const pieceCheckboxes = document.querySelectorAll("input[name^='Pieces']");
    pieceCheckboxes.forEach(function (checkbox) {
        checkbox.addEventListener("change", function () {
            const pieceId = checkbox.id.split('_')[1]; // ID de la pièce
            const quantityContainer = document.getElementById(`quantityContainer_${pieceId}`);

            // Affiche/Masque le champ de quantité
            quantityContainer.style.display = checkbox.checked ? "block" : "none";
        });
    });
});
