using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour {

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PreprocessNotes()
    {
        throw new System.NotImplementedException();
    }

    public void Initialize()
    {
        throw new System.NotImplementedException();
    }

    //Check note tracks for misses, if miss, send HitData with miss to SongGameplayManager, update note track indices

    //Check for held status of current hold notes, if not held, send HitData with miss to SongGameplayManager, update note track indices
    
    // Updates list playhead.
    private void UpdateNoteList(double time) {
        
    }

    // Registers input and checks against note lists to see if it's a valid hit or hold.
    public void RegisterHit(double time, int type, bool release) {
        
    }
    
    // Registers a miss from non-input and passes to scoring.
    private void RegisterMiss() {
        
    }
    
    // Iterate over all inputs and compare to holds.
    private void CheckHold() {
        
    }
    
    // Pass a hit to scoring.
    private void PassToScoring(double hitTime, double noteTime, int type) {
        
    }

    // Pass a miss to scoring.
    private void PassToScoring(double noteTime, int type) {
        
    }
}
