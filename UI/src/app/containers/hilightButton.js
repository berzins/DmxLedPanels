import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { hilightStateReducer } from '../reducers/stateReducer'
import { getHighlightState, enableHighlight } from '../actions/stateActions'
import { selectionReducer } from '../reducers/selectionReducer'


class HighlightButton extends Component {


    componentDidMount() {
        this.props.getHighlightState()
    }

    handleClick() {
        this.props.click.clicked = true
        this.props.enableHighlight(!this.props.highlight.on)

    }

    getLook() {
        return this.props.highlight.on ? "btn-warning" : "btn-outline-warning"
    }

    render() {
        const look = this.getLook()

        return(
            <div
            className={"button btn util-button btn-margin-right " + look}
            autoComplete="off"
            onClick={ () => this.handleClick() }
            >
                <div><h3><b>Highlight</b></h3></div>
            </div>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        highlight: state.hilightStateReducer
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators(
        {
            getHighlightState: getHighlightState,
            enableHighlight: enableHighlight
        }
        , dispatch
    )
}

export default connect(mapStateToProps, mapDispatchToProps)(HighlightButton)