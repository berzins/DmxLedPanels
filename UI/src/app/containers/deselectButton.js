import React, { Component } from 'react'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import { deselectAll } from '../actions/selectActions'
import { selectionReducer } from '../reducers/selectionReducer'

class DeselectButton extends Component {

    constructor(props) {
        super(props) 
        this.enabled = false
    }

    shouldComponentUpdate(nextProps, nextState) {
        const she = this.shouldBeEnabled(nextProps.selection)
        if(this.enabled != she) {
            this.enabled = !this.enabled
            return true
        }
        return false
    }

    shouldBeEnabled(selection) {
        return (selection.hasFixture || selection.hasOutput) ? true : false 
    }

    handleClick() {
        this.props.click.clicked = true
        this.props.deselectAll()
    }

    render() {
        return(
            <div
            style={this.props.style}
            className={"button btn util-button btn-margin-right " + (this.enabled ? "btn-info" : "btn-outline-dark")}
            autoComplete="off"
            onClick={ () => this.handleClick()}
            >
                <div><h3><b>Unselect All</b></h3></div>
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
        deselectAll: deselectAll
    }, dispatch)
}

export default connect(mapStateToProps,mapDispatchToProps)(DeselectButton);