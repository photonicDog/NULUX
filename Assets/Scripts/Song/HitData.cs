

public class HitData {
    public float time;
    public float offset;
    public ScoringHeuristic heur;

    public HitData(float time, float offset, ScoringHeuristic heur) {
        this.time = time;
        this.offset = offset;
        this.heur = heur;
    }
}

