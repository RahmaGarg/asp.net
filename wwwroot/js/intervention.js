document.addEventListener("DOMContentLoaded", function () {
    const checkboxNecessitePieces = document.getElementById("NecessitePieces");
    const hiddenNecessitePieces = document.getElementById("NecessitePiecesHidden");

    // Mettre à jour le champ caché en fonction de l'état de la case à cocher
    checkboxNecessitePieces.addEventListener("change", function () {
        hiddenNecessitePieces.value = checkboxNecessitePieces.checked ? "true" : "false";
    });

    // Logique pour afficher/masquer le container des pièces en fonction de la case
    const piecesContainer = document.getElementById("piecesContainer");
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
            quantityContainer.style.display = checkbox.checked ? "block" : "none";
        });
    });
});
