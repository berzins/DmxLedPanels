import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import Output from './output'
import { deselectAll } from '../actions/selectActions'


class OutputPool extends Component {

    createOutputItems(outputs) {
        return outputs.map((out, i) => {
            return(
                <div className="col-12" key={"out" + out.ID}>
                    <Output output={out} click={this.props.click}/>
                </div>
            )
        })
    }

    handleClick() {
        this.props.click.clicked = true
    }

    render() {
        var style = {
            paddingBottom: '20vh',
            borderBottom: 'solid',
            borderTop: 'solid'
        }
        return(
            <div 
            onClick={() => this.handleClick() }
            className="output output-pool"
            style={style}>
            <div className="row">
                    {this.createOutputItems(this.props.outputs)}
            </div>
            </div>
        );
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators( 
        {
            deselectAll: deselectAll
        }, dispatch)
}

export default connect(null, mapDispatchToProps)(OutputPool);