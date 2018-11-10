import React, { Component } from 'react'
import Fixture from './fixture'
import OutputButton from './outputButton'


class Output extends Component {


    handleClick() {
        this.props.click.clicked = true
    }


    renderFixtures(fixtures) {
        return fixtures.map((fix, i) => {
            return(
                <div key={"fix" + fix.Id} 
                className=" 
                col-6
                col-lg-3">
                    <Fixture fixture={fix} click={this.props.click}/>
                </div>
            )
        })
    }

    render() {
        return(
            <div 
            className="jumbotron output border border-primary output-button"
            onClick={() => this.handleClick()}
            >
            <div className="row align-items-center">

                <div className=" 
                col-12
                col-sm-12
                col-md-3">
                    <OutputButton 
                    data={{
                        id: this.props.output.ID, 
                        name: this.props.output.Name, 
                        ports: this.props.output.Ports,
                        ip: this.props.output.IPAddress,
                        fixtures: this.props.output.Fixtures
                    }}
                    click={this.props.click}
                    />
                </div>
                
                <div className="  
                col-12
                col-sm-12
                col-md-9
                "
                >
                    <div className="row">
                        {this.renderFixtures(this.props.output.Fixtures)}                                
                    </div>
                </div>
                
            </div>
            </div>
        );
    }
}

export default Output;