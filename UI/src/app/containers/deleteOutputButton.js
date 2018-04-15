import React, { Component } from 'react'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import {deleteOutput} from '../actions/stateActions'
import { deselectAll } from '../actions/selectActions'
import { selectionReducer } from '../reducers/selectionReducer'


class DeleteOutputButton extends Component {

    handleClick() {
        this.props.click.clicked = true
        this.props.deleteOutput(this.props.selection.outputs)
        this.props.deselectAll()
    }

    render() {
        let s = this.props.selection
        return(
            <div
            className={"button btn btn-danger item-button " + (!s.onlyOutput ? 'disabled' : '')} 
            autoComplete="off"
            onClick={() => this.handleClick()}
            >
                <div><b>Delete Output</b></div>
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
        deleteOutput: deleteOutput,
        deselectAll: deselectAll
    }, dispatch)
}

export default connect(mapStateToProps,mapDispatchToProps)(DeleteOutputButton);