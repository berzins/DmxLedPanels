import { API_URL } from '../constants/const'
import { bindActionCreators } from 'redux'
import store from '../store'
import { 
    getUpdateDmxStateDispatchObject,
    getUpdatePortDmxStateDispatchObject
 } from '../actions/stateActions'

export class RemoteEventManager {
    
    static instance = null

    static getInstance(){
        if(RemoteEventManager.instance == null) {
            RemoteEventManager.instance = new RemoteEventManager()
        }
        return this.instance
    }

    constructor() {
        this.evnetSource = new EventSource(API_URL + '/events/');
        this.evnetSource.addEventListener("dmx_sginal", e => {
            this.handleDmxSignalChange(JSON.parse(e.data))
        })
        this.evnetSource.addEventListener("dmx_signal_port", e => {
            this.handlePortDmxSignalChange(JSON.parse(e.data))
        })
    }

    handleDmxSignalChange(data) {
        let hasSignal = data.has_dmx_sginal
        if(hasSignal == 'True' || hasSignal == 'False') {
            hasSignal = hasSignal == 'True' ? true : false
            store.dispatch(getUpdateDmxStateDispatchObject(hasSignal))
        }
    }

    handlePortDmxSignalChange(data) {
        let ports = JSON.parse(data.dmx_active_port);
        store.dispatch(getUpdatePortDmxStateDispatchObject(ports))
    }
}

