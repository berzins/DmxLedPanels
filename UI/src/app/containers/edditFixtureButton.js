import React, { Component } from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import { 
    openEditFixtureNameForm,
    openEditFixtureAddressForm,
    openEditFixtureModeForm,
    openEditFixturePatchForm
} from '../actions/formActions'
import { selectionReducer } from '../reducers/selectionReducer'

const ITEM_NAME = "ITEM_NAME"
const ITEM_ADDRESS = "ITEM_ADDRESS"
const ITEM_MODE = "ITEM_MODE"
const ITEM_PATCH = "ITEM_PATCH"


class EditFixtureButton extends Component {

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
        return props.selection.hasFixture ? true : false
    }

    handleClick(item) {
        this.props.click.clicked = true
        if(this.enabled) {
            switch(item) {
                case ITEM_NAME: {
                    this.props.openEditFixtureNameForm();
                    break;
                }
                case ITEM_ADDRESS: {
                    this.props.openEditFixtureAddressForm();
                    break;
                }
                case ITEM_MODE: {
                    this.props.openEditFixtureModeForm()
                    break;
                }
                case ITEM_PATCH: {
                    this.props.openEditFixturePatchForm()
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
            id="dropdownMenuButton1"
            type="button"
            className={"btn btn-warning item-button dropdown-toggle " + 
            (this.enabled ? "" : "disabled")} 
            data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"
            //autoComplete="off"
            >
                <b>Edit Fixture</b>
            </button>
            <div className="dropdown-menu" aria-labelledby="dropdownMenuButton1">
                <a className="dropdown-item" onClick={() => this.handleClick(ITEM_NAME)} href="javascript:void(0)">Name</a>
                <a className="dropdown-item" onClick={() => this.handleClick(ITEM_ADDRESS)}href="javascript:void(0)">Address</a>
                <a className="dropdown-item" onClick={() => this.handleClick(ITEM_MODE)}href="javascript:void(0)">Mode</a>
                <a className="dropdown-item" onClick={() => this.handleClick(ITEM_PATCH)}href="javascript:void(0)">Patch</a>
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
        openEditFixtureNameForm: openEditFixtureNameForm,
        openEditFixtureAddressForm, openEditFixtureAddressForm,
        openEditFixtureModeForm: openEditFixtureModeForm,
        openEditFixturePatchForm: openEditFixturePatchForm
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(EditFixtureButton);