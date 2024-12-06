document.addEventListener("DOMContentLoaded", function () {
    const checkbox = document.getElementById("NecessitePieces");
    const piecesContainer = document.getElementById("piecesContainer");

    // Vérifiez si la case est déjà cochée au chargement
    piecesContainer.style.display = checkbox.checked ? "block" : "none";

    // Ajoutez un écouteur d'événement pour le changement de l'état de la case
    checkbox.addEventListener("change", function () {
        piecesContainer.style.display = checkbox.checked ? "block" : "none";
    });

    // Calculer le coût total dès que la durée de l'intervention ou les pièces sont modifiées
    const dureeIntervention = document.getElementById("DureeIntervention");
    const coutTotalInput = document.getElementById("CoutTotal");

    dureeIntervention.addEventListener("input", function () {
        const duree = parseFloat(dureeIntervention.value) || 0;
        let coutTotal = duree * tarifHoraire; // tarifHoraire doit être défini dans la vue ou dans le script

        // Ajouter le coût des pièces
        const selectedPieces = document.querySelectorAll("input[name^='Pieces']:checked");
        selectedPieces.forEach(function (piece) {
            const prix = parseFloat(piece.closest('.form-check').querySelector("input[name^='Pieces'][name$='.Prix']").value) || 0;
            coutTotal += prix;
        });

        // Mettre à jour le champ de coût total
        coutTotalInput.value = coutTotal.toFixed(2);
    });
});
