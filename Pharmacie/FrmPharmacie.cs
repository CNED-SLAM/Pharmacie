using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Pharmacie
{
    /// <summary>
    /// Classe FrmPharmacie : affichage des médicaments, ajoute et suppression
    /// </summary>
    public partial class FrmPharmacie : Form
    {
        private const string connectionString = "mongodb://127.0.0.1:27017";
        private readonly MongoClient client = new MongoClient(connectionString);
        private IMongoDatabase db;
        private IMongoCollection<Medicament> colMedicaments;
        private IMongoCollection<Recommandation> colRecommandations;
        private readonly BindingSource bdgMedicaments = new BindingSource();
        private readonly BindingSource bdgRecommandations = new BindingSource();

        /// <summary>
        /// Consutructeur : initialisation des objets graphiques et appel de la méthode Init
        /// </summary>
        public FrmPharmacie()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Initialisation des objets de connexion à la BDD
        /// appel des méthodes de chargement des listes graphiques
        /// </summary>
        private void Init()
        {
            db = client.GetDatabase("pharmacie");
            colMedicaments = db.GetCollection<Medicament>("medicaments");
            colRecommandations = db.GetCollection<Recommandation>("recommandations");
            LoadMedicaments();
            LoadRecommandations();
        }

        /// <summary>
        /// Chargement du grid des médicaments
        /// </summary>
        private void LoadMedicaments()
        {
            dgvMedicaments.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing; // pour améliorer le temps de chargement
            List<Medicament> lesMedicaments = colMedicaments.AsQueryable().ToList<Medicament>();
            lesMedicaments.Sort();
            bdgMedicaments.DataSource = lesMedicaments;
            dgvMedicaments.DataSource = bdgMedicaments;
            dgvMedicaments.Columns["Id"].Visible = false;
            dgvMedicaments.Columns["libelle"].HeaderText = "libellé ATC3";
            dgvMedicaments.Columns["forme"].HeaderText = "forme galénique";
            dgvMedicaments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Chargement du combo des recommandations pour l'ajout d'un médicament
        /// et de la liste des recommandations (information non modifiable)
        /// </summary>
        private void LoadRecommandations()
        {
            List<Recommandation> lesRecommandations = colRecommandations.AsQueryable().ToList<Recommandation>();
            bdgRecommandations.DataSource = lesRecommandations;
            cboRecommandations.DataSource = bdgRecommandations;
            lstRecommandations.DataSource = bdgRecommandations;
        }

        /// <summary>
        /// Evénement clic sur le bouton supprimer : demande de suppression du médicament sélectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSuppr_Click(object sender, EventArgs e)
        {
            if (dgvMedicaments.SelectedRows.Count > 0)
            {
                Medicament medicament = (Medicament)bdgMedicaments.List[bdgMedicaments.Position];
                if (MessageBox.Show("Voulez vous vraiment supprimer " + medicament.nom + " ?", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SupprMedicament(medicament);
                    LoadMedicaments();
                }
            }
            else
            {
                MessageBox.Show("Une ligne doit être sélectionnée.", "Information");
            }
        }

        /// <summary>
        /// Suppression d'un médicament
        /// </summary>
        /// <param name="medicament"></param>
        private void SupprMedicament(Medicament medicament)
        {
            FilterDefinition<Medicament> filter = Builders<Medicament>.Filter.Eq("Id", medicament.Id);
            colMedicaments.DeleteOne(filter);
        }

        /// <summary>
        /// Evénement clic sur le bouton ajouter : demande d'ajout d'un médicament
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAjout_Click(object sender, EventArgs e)
        {
            if (!txtForme.Text.Equals("") && !txtLibelle.Text.Equals("") && !txtNom.Text.Equals(""))
            {
                AjoutMedicament();
                LoadMedicaments();
                ReinitialiseZoneSaisie();
            }
            else
            {
                MessageBox.Show("Tous les champs doivent être remplis.", "Information");
            }
        }

        /// <summary>
        /// Ajout d'un médicament dans la BDD à partir des informations saisies
        /// </summary>
        private void AjoutMedicament()
        {
            Recommandation recommandation = (Recommandation)bdgRecommandations.List[bdgRecommandations.Position];
            Medicament medicament = new Medicament(txtNom.Text, txtLibelle.Text, txtForme.Text, recommandation.code);
            colMedicaments.InsertOne(medicament);
        }

        /// <summary>
        /// Réinitialise la zone de saisie du médicament en vidant les champs
        /// </summary>
        private void ReinitialiseZoneSaisie()
        {
            txtForme.Text = "";
            txtLibelle.Text = "";
            txtNom.Text = "";
        }

    }
}
