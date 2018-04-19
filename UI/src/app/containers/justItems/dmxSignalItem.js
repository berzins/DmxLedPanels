import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { getDmxState } from '../../actions/stateActions'
import { dmxStateReducer } from '../../reducers/stateReducer'

class DmxSignalItem extends Component {

    render() {
        this.props.getDmxState(this.props.hasSignal);
        return(
            <div 
            className={"badge " + (this.props.hasSignal ? "badge-success" : "badge-danger")}
            style={{width:100+'%'}}
            >
            {this.props.hasSignal ? "dmx on" : "dmx off"}
            </div>
        )
    }
}

const mapStateToProps = state => {
    return {
        hasSignal: state.dmxStateReducer
    }
}

const mapDispatchToProps = dispatch => {
    return bindActionCreators(
        {
            getDmxState: getDmxState
        }, dispatch
    )
}

export default connect(mapStateToProps, mapDispatchToProps)(DmxSignalItem)