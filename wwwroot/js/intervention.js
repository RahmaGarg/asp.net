document.addEventListener("DOMContentLoaded", function () {
    const checkbox = document.getElementById("NecessitePieces");
    const piecesContainer = document.getElementById("piecesContainer");

    // Vérifiez si la case est déjà cochée au chargement
    piecesContainer.style.display = checkbox.checked ? "block" : "none";

    // Ajoutez un écouteur d'événement pour le changement de l'état de la case
    checkbox.addEventListener("change", function () {
        piecesContainer.style.display = checkbox.checked ? "block" : "none";
    });

    // Ajoutez un écouteur d'événements pour chaque case à cocher de pièce
    const pieceCheckboxes = document.querySelectorAll("input[name^='Pieces']");
    pieceCheckboxes.forEach(function (checkbox) {
        checkbox.addEventListener("change", function () {
            const pieceId = checkbox.id.split('_')[1]; // Récupérer l'ID de la pièce
            const quantityContainer = document.getElementById(`quantityContainer_${pieceId}`);

            // Afficher ou cacher le champ de quantité en fonction de l'état de la case
            quantityContainer.style.display = checkbox.checked ? "block" : "none";
        });
    });
});
