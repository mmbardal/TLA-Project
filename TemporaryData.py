import collections

completedStates = []
discoveredButNotCompletedStates = []
finalStates=[]

def CheckIsThisStateDiscoveredOrBuilt(state):
    if(IsItBuiltOrNot(state)):
        return False
    if(IsItDiscoveredOrNot(state)):
        return False
    return True

def IsItBuiltOrNot(state):
    for item in completedStates:
        if collections.Counter( list(set(item["states"]))) ==collections.Counter( list(set(state))):
            return True
    return False

def IsItDiscoveredOrNot(state):
    for item in discoveredButNotCompletedStates:
        if collections.Counter( list(set(item))) ==collections.Counter( list(set(state))):
            return True
    return False

def EnqueueState(state):
    if not IsItBuiltOrNot(state):
        if not IsItDiscoveredOrNot(state):
            discoveredButNotCompletedStates.append(state)
