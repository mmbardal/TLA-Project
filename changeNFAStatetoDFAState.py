import json
import ReadJSONFile
import QueryInOriginalData
import OriginalData
import TemporaryData


def CheckStateExistence(
        allCreatedStates,
        stateToBeChecked):
    for item in allCreatedStates.keys():
        if CheckListsEquality(item, stateToBeChecked):
            return True

    return False


def CheckListsEquality(list1, list2):
    list1.sort()
    list2.sort()
    if list1 == list2:
        return True
    else:
        return False


def ToWhichStatesDoesThisInputStateGoWithDifferentTransitions(
        inputState,
        input_symbols
):

    destinationStatesWithDifferentTransitions = {}
    for alphabet in input_symbols:
        destinationStatesWithDifferentTransitions[alphabet] = []
        for item in inputState["Transitions"]:
            destinationStatesWithDifferentTransitions[alphabet].append(item)
    return destinationStatesWithDifferentTransitions


def CreateNewInitialState(
        states,
        transitions,
        intial_state,
        final_states
):
    newInitialStates = FindLambdaTransitions(intial_state)
    newInitialStates.append(intial_state)
    newTransitions = {}

    for alphabet in OriginalData.input_symbols:
        newTransitions[alphabet] = []

        for state in newInitialStates:
            if alphabet in transitions[state]:
                for obj in transitions[state][alphabet]:
                    newTransitions[alphabet].append(obj)
                    for i in FindLambdaTransitions(obj):
                        newTransitions[alphabet].append(i)
        if (len(newTransitions[alphabet]) == 0):
            newTransitions[alphabet].append(OriginalData.Trap)
        newTransitions[alphabet] = list(set(newTransitions[alphabet]))
        TemporaryData.EnqueueState(list(set(newTransitions[alphabet])))

    newInitialState = {
        "states": newInitialStates,
        "Transition": newTransitions
    }
    TemporaryData.completedStates.append(newInitialState)
    return newInitialState


def FindLambdaTransitions(
        inputState
):
    if inputState == OriginalData.Trap:
        return []
    states = []
    lambda_symbol = ''
    if lambda_symbol in OriginalData.originalTransitions[inputState]:
        for item in OriginalData.originalTransitions[inputState][lambda_symbol]:
            states += FindLambdaTransitions(item)
            states.append(item)
    return states



def CreateNewNonInitialState(
        states,
        transitions,
        initial_state,
        final_states):
    CreateNewInitialState(states, 
                          transitions, initial_state, final_states)
    while len(TemporaryData.discoveredButNotCompletedStates) != 0:
        newState = TemporaryData.discoveredButNotCompletedStates.pop()
        e = []
        for k in newState:
            e.append(k)
        for s in newState:
            for item in FindLambdaTransitions(s):
                e.append(item)
        if (TemporaryData.CheckIsThisStateDiscoveredOrBuilt(e)):
            it = {}
            it["states"] = list(set(e))
            it["Transition"] = QueryInOriginalData.ReturnTransitionsForMultipleInputSymbolAndMultipleStates(
                e)
            if(ReadJSONFile.IsItFinalState(it["states"])):
                it["finalState"]="Yes"
            else:
                it["finalState"]="No"
            TemporaryData.completedStates.append(it)
            for item in it["Transition"].keys():
                if it["Transition"][item][0]== OriginalData.Trap:
                    continue
                if(TemporaryData.CheckIsThisStateDiscoveredOrBuilt(it["Transition"][item])):
                    TemporaryData.discoveredButNotCompletedStates.append(it["Transition"][item])
    ReadJSONFile.WriteToJSONFile()
