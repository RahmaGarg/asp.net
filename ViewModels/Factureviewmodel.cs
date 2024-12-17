namespace Atelier_2.ViewModels
{
    public class FactureViewModel
    {
        public string TechnicienNom { get; set; }
        public decimal DureeIntervention { get; set; }
        public decimal CoutHoraire { get; set; }
        public List<FacturePiece> Pieces { get; set; }
        public decimal CoutTechnicien { get; set; }
        public decimal CoutTotal { get; set; }
    }

    public class FacturePiece
    {
        public string Nom { get; set; }
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }

        // Propriété calculée
        public decimal PrixTotal => Quantite * PrixUnitaire;
    }


}
