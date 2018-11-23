/* CLASSE PROFILO PAZIENTE 
*  Classe che permette di creare il profilo del paziente
* con tutti i dari necessari al fisioterapista per gli esercizi
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Desktop
{
    [System.Serializable]
    public class PatientProfile
    {
        private string ID;

        public string IDPatient
        {
            get { return ID; }
            set { ID = value; }
        }

        private string Name;

        public string NamePatient
        {
            get { return Name; }
            set { Name = value; }
        }

        private string Surname;

        public string SurnamePatient
        {
            get { return Surname; }
            set { Surname = value; }
        }

        private string Gender;

        public string GenderPatient
        {
            get { return Gender; }
            set { Gender = value; }
        }

        private float Height;

        public float HeightPatient
        {
            get { return Height; }
            set { Height = value; }
        }

        private bool Agonistic;

        public bool AgonisticPatient
        {
            get { return Agonistic; }
            set { Agonistic = value; }
        }

        private string Disability;

        public string DisabilityPatient
        {
            get { return Disability; }
            set { Disability = value; }
        }

        public PatientProfile() { }

        public PatientProfile(string ID, string Name, string Surname, string Gender, 
            float Height, bool Agonistic, string Disability)
        {
            this.ID = ID;
            this.Name = Name;
            this.Surname = Surname;
            this.Gender = Gender;
            this.Height = Height;
            this.Agonistic = Agonistic;
            this.Disability = Disability;
        }
    }

}
