import changeNFAStatetoDFAState
import OriginalData
from ReadJSONFile import WriteToJSONFile

changeNFAStatetoDFAState.CreateNewNonInitialState(
    OriginalData.originalStates,
    OriginalData.originalTransitions,
    OriginalData.initial_state,
    OriginalData.final_states
)
WriteToJSONFile()

