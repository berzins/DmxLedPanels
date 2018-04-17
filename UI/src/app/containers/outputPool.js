import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import Output from './output'
import AddOutputButton from './addOutputButton'
import DeleteOutputButton from './deleteOutputButton'
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
        return(
            <div 
            onClick={() => this.handleClick() }
            className="output"
            >
                <div className="row">
                    <div className='col-12 btn-group'>
                            <AddOutputButton click={this.props.click}/>
                            <DeleteOutputButton click={this.props.click}/>
                    </div> 
                    <div className="col-12">
                        <div className="row">
                            {this.createOutputItems(this.props.outputs)}
                        </div>
                    </div>
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