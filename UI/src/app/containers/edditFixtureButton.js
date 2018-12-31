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
import { storeFixtureTemplate } from '../actions/stateActions'

const ITEM_NAME = "ITEM_NAME"
const ITEM_ADDRESS = "ITEM_ADDRESS"
const ITEM_MODE = "ITEM_MODE"
const ITEM_PATCH = "ITEM_PATCH"
const ITEM_STORE_TEMPLATE = "ITEM_STORE_TEMPLATE"


class EditFixtureButton extends Component {

    constructor(props) {
        super(props)
        this.enabled = this.shouldBeEnabled(this.props)
        this.templateEnabled = false
    }

    shouldComponentUpdate(nextProps, nextState) {
        if(this.enabled != this.shouldBeEnabled(nextProps)) {
            this.enabled = !this.enabled
            return true
        }
        if(this.templateEnabled != this.shouldEnableTemplate(nextProps)) {
            this.templateEnabled = !this.templateEnabled
            return true
        }
        return false
    }

    shouldBeEnabled(props) {
        return props.selection.hasFixture ? true : false
    }

    shouldEnableTemplate(props) {
        return props.selection.length == 1
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
                case ITEM_STORE_TEMPLATE: {
                    this.props.storeFixtureTemplate(this.props.selection.fixtures[0])
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
                <a 
                className={this.shouldEnableTemplate(this.props) ? "dropdown-item" : "dropdown-item disabled"} 
                onClick={() => this.handleClick(ITEM_STORE_TEMPLATE)}
                href="javascript:void(0)">Store template</a>
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
        openEditFixturePatchForm: openEditFixturePatchForm,
        storeFixtureTemplate: storeFixtureTemplate
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(EditFixtureButton);