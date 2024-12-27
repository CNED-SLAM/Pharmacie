using MongoDB.Bson;
using System;

namespace Pharmacie
{
    /// <summary>
    /// Classe métier Medicament
    /// </summary>
    public class Medicament : IComparable
    {
        /// <summary>
        /// Constructeur : valorise les propriétés
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="libelleATC3"></param>
        /// <param name="forme"></param>
        /// <param name="recommandation"></param>
        public Medicament(string nom, string libelle, string forme, string recommandation)
        {
            this.nom = nom;
            this.libelle = libelle;
            this.forme = forme;
            this.recommandation = recommandation;
        }

        public ObjectId Id { get; set; }
        public string nom { get; set; }
        public string libelle { get; set; }
        public string forme { get; set; }
        public string recommandation { get; set; }

        /// <summary>
        /// Comparaison sur le nom
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return nom.CompareTo(((Medicament)obj).nom);
        }

    }
}

