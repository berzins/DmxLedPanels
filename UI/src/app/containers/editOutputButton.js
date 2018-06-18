import React, { Component } from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import { 
    openEditOutputNameForm,
    openEditOutputPortForm,
    openEditOutputIpForm
} from '../actions/formActions'
import { selectionReducer } from '../reducers/selectionReducer'

const ITEM_NAME = "ITEM_NAME"
const ITEM_PORT = "ITEM_PORT"
const ITEM_IP = "ITEM_IP"


class EditOutputButton extends Component {

    constructor(props) {
        super(props)
        this.enabled = this.shouldBeEnabled(this.props)
    }

    shouldComponentUpdate(nextProps, nextState) {
        if(this.enabled != this.shouldBeEnabled(nextProps)) {
            this.enabled = !this.enabled
            return true
        }
        return false
    }

    shouldBeEnabled(props) {
        return props.selection.hasOutput ? true : false
    }

    handleClick(item) {
        this.props.click.clicked = true
        if(this.enabled) {
            switch(item) {
                case ITEM_NAME: {
                    this.props.openEditOutputNameForm();
                    break;
                }
                case ITEM_PORT: {
                    this.props.openEditOutputPortForm();
                    break;
                }
                case ITEM_IP: {
                    this.props.openEditOutputIpForm();
                    break;
                }
            }
        }
    }

    render() {
        return(
            <div>
            <div className="dropdown">
            <button
            id="editOutputDropdownButton"
            type="button"
            className={"btn btn-warning item-button dropdown-toggle " + 
            (this.enabled ? "" : "disabled")} 
            data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"
            //autoComplete="off"
            >
                <b>Edit Output</b>
            </button>
            <div className="dropdown-menu" aria-labelledby="editOutputDropdownButton123">
                <a className="dropdown-item" onClick={() => this.handleClick(ITEM_NAME)} href="javascript:void(0)">Name</a>
                <a className="dropdown-item" onClick={() => this.handleClick(ITEM_PORT)}href="javascript:void(0)">Port</a>
                <a className="dropdown-item" onClick={() => this.handleClick(ITEM_IP)}href="javascript:void(0)">Ip address</a>
            </div>
            </div>
            </div>
            
        );
    }
}


const mapStateToProps = (state) => {
    return {
        selection: state.selectionReducer
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        openEditOutputNameForm: openEditOutputNameForm,
        openEditOutputPortForm: openEditOutputPortForm,
        openEditOutputIpForm: openEditOutputIpForm
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(EditOutputButton);