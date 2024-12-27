using Pharmacie.model;
using Pharmacie.bddmanager;
using System.Collections.Generic;
using System;

namespace Pharmacie.controller
{
    /// <summary>
    /// Contrôleur de FrmPharmacie
    /// </summary>
    public class Controller
    {
        private const string connectionString = "mongodb://127.0.0.1:27017";
        private const string dataBase = "pharmacie";
        private const string nomColRecommandations = "recommandations";
        private const string nomColMedicaments = "medicaments";
        private readonly BddManager bddManager;

        /// <summary>
        /// Constructeur : récupère l'instance pour la connection à la BDD
        /// </summary>
        public Controller()
        {
            try
            {
                this.bddManager = BddManager.GetInstance(connectionString, dataBase);
            }
            catch
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Récupère la liste des médicaments dans la BDD
        /// </summary>
        /// <returns>liste de médicaments</returns>
        public List<Medicament> GetMedicaments()
        {
            return bddManager.CollectionToList<Medicament>(nomColMedicaments);
        }

        /// <summary>
        /// Récupère la liste des recommandations dans la BDD
        /// </summary>
        /// <returns>liste des recommandations</returns>
        public List<Recommandation> GetRecommandations()
        {
            List<Recommandation> liste = bddManager.CollectionToList<Recommandation>(nomColRecommandations);
            if (liste.Count == 0)
            {
                Console.WriteLine("Collection " + nomColRecommandations + " vide ou inexistante.");
                Environment.Exit(0);
            }
            return liste;
        }

        /// <summary>
        /// Demande d'ajout d'un médicament dans la BDD
        /// </summary>
        /// <param name="medicament"></param>
        public void AjoutMedicament(Medicament medicament)
        {
            bddManager.Ajout<Medicament>(nomColMedicaments, medicament);
        }

        /// <summary>
        /// Demande de suppression d'un médicament de la BDD
        /// </summary>
        /// <param name="medicament"></param>
        public void SupprMedicament(Medicament medicament)
        {
            bddManager.Suppr<Medicament>(nomColMedicaments, medicament.Id);
        }

    }
}
