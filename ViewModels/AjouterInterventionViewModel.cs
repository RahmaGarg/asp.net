using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Atelier_2.ViewModels
{
    public class PieceSelectionViewModel
    {
        public int PieceId { get; set; }
        public string Nom { get; set; }
        public bool IsChecked { get; set; }
        public int Quantity { get; set; }
        public decimal Prix { get; set; } // Prix de la pièce

    }

    public class AjouterInterventionViewModel
    {
        public int ReclamationId { get; set; }
        public DateTime DateIntervention { get; set; }
        public string TechnicienId { get; set; }
        public decimal DureeIntervention { get; set; }
        public bool NecessitePieces { get; set; }
        public decimal CoutTotal { get; set; }
        public List<SelectListItem> Techniciens { get; set; } = new List<SelectListItem>();
        // Liste des pièces disponibles et sélectionnées
        public List<PieceSelectionViewModel> Pieces { get; set; } = new List<PieceSelectionViewModel>();
    }


}