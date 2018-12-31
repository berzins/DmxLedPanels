import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { dmxStateReducer } from '../../reducers/stateReducer'

class DmxSignalItem extends Component {

    render() {
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
            
        }, dispatch
    )
}

export default connect(mapStateToProps, mapDispatchToProps)(DmxSignalItem)