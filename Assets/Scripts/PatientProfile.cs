/* CLASSE PROFILO PAZIENTE 
*  Classe che permette di creare il profilo del paziente
* con tutti i dari necessari al fisioterapista per gli esercizi
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatientProfile {
	
	private string ID;

	public string IDPatient {
		get {return ID;}
		set {ID = value;}
	}

	private string Name;

	public string namePatient {
		get {return Name;}
		set {Name = value;}
	}

	private string Surname;

	public string surnamePatient {
		get {return Surname;}
		set {Surname = value;}
	}

	private string CauseDisability;

	public string causeDisabilityPatient {
		get {return CauseDisability;}
		set {CauseDisability = value;}
	}

	private string Disability;

	public string disabilityPatient {
		get {return  Disability;}
		set {Disability = value;}
	}

	public PatientProfile(string ID, string Name, string Surname, string CauseDisability, string Disability) {
		this.ID = ID;
		this.Name = Name;
		this.Surname = Surname;
		this.CauseDisability = CauseDisability;
		this.Disability = Disability;
	}

	public List<string> GetInfoPatient() {
		List<string> list = new List<string>();
		list.Add(IDPatient);
		list.Add(namePatient);
		list.Add(surnamePatient);
		list.Add(causeDisabilityPatient);
		list.Add(disabilityPatient);
		return list;


	}
}
