using System.Collections.Generic;

[System.Serializable]
public class Chromosome {

    public ChromosomeMonobehaviour ChromosomeMonobehaviour;

    public List<float> GeneList = new List<float>();

    public float MinTorque;

    public float MaxTorque;
}
