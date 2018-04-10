import React, { Component } from 'react';
import Output from './output'
import AddOutputButton from './addOutputButton'


class OutputPool extends Component {

    createOutputItems(count) {
        let outs = []
        for(let i = 0; i < count; i++) {
            outs.push(
                <div className="col-12">
                    <Output />
                </div>
            )
        }
        return outs
    }

    render() {
        return(
            <div className="output border border-primary">
                <div className="row">

                    <div className="col-12">
                        <AddOutputButton/>
                    </div>
                    <div className="col-12">
                        <div className="row">
                            {this.createOutputItems(0)}
                        </div>
                    </div>
                </div>      
            </div>
        );
    }
}

export default OutputPool;