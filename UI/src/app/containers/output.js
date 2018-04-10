import React, { Component } from 'react';
import Fixture from './fixture'
import OutputButton from './outputButton'


class Output extends Component {

    createFixtureItems(count){
        let fix = []
        for(let i = 0; i < count; i++) {
            fix.push(
                <div className=" 
                    col-6
                    col-lg-3">
                        <Fixture />
                    </div>
            )
        }
        return fix
    }

    render() {
        return(
            <div className="jumbotron output border border-dark">
            <div className="row align-items-center">

                <div className=" 
                col-12
                col-sm-12
                col-md-3">
                <OutputButton/>
                </div>
                
                <div className="  
                col-12
                col-sm-12
                col-md-9
                "
                >
                    <div className="row">
                        {this.createFixtureItems(4)}                                
                    </div>
                </div>
                
            </div>
            </div>
        );
    }
}

export default Output;