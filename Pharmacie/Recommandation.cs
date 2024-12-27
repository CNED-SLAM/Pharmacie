using MongoDB.Bson;

namespace Pharmacie
{
    /// <summary>
    /// Classe métier recommandation
    /// </summary>
    public class Recommandation
    {

        /// <summary>
        /// Constructeur : valorise les propriétés
        /// </summary>
        /// <param name="code"></param>
        /// <param name="libelle"></param>
        public Recommandation(string code, string libelle)
        {
            this.code = code;
            this.libelle = libelle;
        }

        public ObjectId Id { get; }
        public string code { get; set; }
        public string libelle { get; set; }

        /// <summary>
        /// Chaîne de construction pour l'affichage
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(code))
            {
                return libelle;
            }
            else
            {
                return code + " : " + libelle;
            }
        }

    }
}
