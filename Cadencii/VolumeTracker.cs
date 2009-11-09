﻿/*
 * VolumeTracker.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.Cadencii;

import javax.swing.*;
import org.kbinani.*;
#else
using System;
using System.Windows.Forms;
using bocoree;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using BEventArgs = System.EventArgs;
    using BKeyEventArgs = System.Windows.Forms.KeyEventArgs;
    using boolean = System.Boolean;
#endif

#if JAVA
    public class VolumeTracker extends JPanel {
#else
    class VolumeTracker : UserControl {
#endif
        private int m_feder = 0;
        private boolean m_solo_button_visible;
        private boolean m_muted = false;
        private boolean m_solo = false;
        private String m_number = "0";
        private String m_title = "";
        private Object m_tag = null;

        #region Constants
        public const int WIDTH = 85;
        const int _HEIGHT = 261;
#if JAVA
        private static final int[][] _KEY = {
#else
        private static readonly int[,] _KEY = {
#endif
            {55, 26}, 
            {51, 27},
            {47, 28},
            {42, 30},
            {38, 31},
            {35, 33},
            {31, 34},
            {28, 36},
            {24, 37},
            {21, 39},
            {18, 40},
            {15, 42},
            {12, 43},
            {10, 45},
            {7, 46},
            {5, 48},
            {2, 49},
            {0, 51},
            {-2, 52},
            {-5, 54},
            {-7, 55},
            {-10, 57},
            {-12, 58},
            {-15, 60},
            {-18, 61},
            {-21, 63},
            {-24, 64},
            {-28, 66},
            {-31, 67},
            {-35, 69},
            {-38, 70},
            {-42, 72},
            {-47, 73},
            {-51, 75},
            {-55, 76},
            {-60, 78},
            {-65, 79},
            {-70, 81},
            {-76, 82},
            {-81, 84},
            {-87, 85},
            {-93, 87},
            {-100, 88},
            {-107, 89},
            {-114, 91},
            {-121, 92},
            {-129, 94},
            {-137, 95},
            {-145, 97},
            {-154, 98},
            {-163, 100},
            {-173, 101},
            {-183, 103},
            {-193, 104},
            {-204, 106},
            {-215, 107},
            {-227, 109},
            {-240, 110},
            {-253, 112},
            {-266, 113},
            {-280, 115},
            {-295, 116},
            {-311, 118},
            {-327, 119},
            {-344, 121},
            {-362, 122},
            {-380, 124},
            {-399, 125},
            {-420, 127},
            {-441, 128},
            {-463, 130},
            {-486, 131},
            {-510, 133},
            {-535, 134},
            {-561, 136},
            {-589, 137},
            {-617, 139},
            {-647, 140},
            {-678, 142},
            {-711, 143},
            {-745, 145},
            {-781, 146},
            {-818, 148},
            {-857, 149},
            {-898, 151},
        };
        #endregion

#if JAVA
        public BEvent<BEventHandler> federChangedEvent = new BEvent<BEventHandler>();
        public BEvent<BEventHandler> panpotChangedEvent = new BEvent<BEventHandler>();
        public BEvent<BEventHandler> isMutedChangedEvent = new BEvent<BEventHandler>();
        public BEvent<BEventHandler> isSoloChangedEvent = new BEvent<BEventHandler>();
#else
        public event EventHandler FederChanged;
        public event EventHandler PanpotChanged;
        public event EventHandler IsMutedChanged;
        public event EventHandler IsSoloChanged;
#endif

        public VolumeTracker() {
#if JAVA
		    super();
		    initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
#if !JAVA
            this.SetStyle( ControlStyles.DoubleBuffer, true );
#endif
        }

        public void setTag( Object value ) {
            m_tag = value;
        }

        public Object getTag() {
            return m_tag;
        }

        public String getTitle() {
            return m_title;
        }

        public void setTitle( String value ) {
            m_title = value;
            updateTitle();
        }

        private void updateTitle() {
            if ( m_number.Equals( "" ) ) {
                lblTitle.setText( m_title );
            } else if ( m_title.Equals( "" ) ) {
                lblTitle.setText( m_number );
            } else {
                lblTitle.setText( m_number + " " + m_title );
            }
        }

        public String getNumber() {
            return m_number;
        }

        public void setNumber( String value ) {
            m_number = value;
            updateTitle();
        }

        public boolean isMuted() {
            return m_muted;
        }

        public void setMuted( boolean value ) {
            boolean old = m_muted;
            m_muted = value;
            if ( old != m_muted ){
#if JAVA
                try{
                    isMuteChangedEvent.raise( this, new BEventArgs() );
                }catch( Exception ex ){
                    System.err.println( "VolumeTracker#setMuted; ex=" + ex );
                }
#else
                if( IsMutedChanged != null ) {
                    IsMutedChanged( this, new EventArgs() );
                }
#endif
            }
        }

        public boolean isSolo() {
            return m_solo;
        }

        public void setSolo( boolean value ) {
            boolean old = m_solo;
            m_solo = value;
            if ( old != m_solo ){
#if JAVA
                try{
                    isSoloChangedEvent.raise( this, new BEventArgs() );
                }catch( Exception ex ){
                    System.err.println( "VolumeTracker#setSolo; ex=" + ex );
                }
#else
                if ( IsSoloChanged != null ) {
                    IsSoloChanged( this, new EventArgs() );
                }
#endif
            }
        }

        public int getPanpot() {
            return trackPanpot.getValue();
        }

        public void setPanpot( int value ) {
            trackPanpot.setValue( value );
        }

        public boolean isSoloButtonVisible() {
            return m_solo_button_visible;
        }

        public void setSoloButtonVisible( boolean value ) {
            m_solo_button_visible = value;
        }

        public int getFeder() {
            return m_feder;
        }

        public void setFeder( int value ) {
            int old = m_feder;
            m_feder = value;
            if ( old != m_feder ){
#if JAVA
                try{
                    federChangedEvent.raise( this, new BEventArgs() );
                }catch( Exception ex ){
                    System.err.println( "VolumeTracker#setFeder; ex=" + ex );
                }
#else
                if ( FederChanged != null ) {
                    FederChanged( this, new EventArgs() );
                }
#endif
            }
            int v = 177 - getYCoordFromFeder( m_feder );
            trackFeder.setValue( v );
        }

        private void VolumeTracker_Resize( Object sender, BEventArgs e ) {
#if !JAVA
            this.Width = WIDTH;
            this.Height = _HEIGHT;
#endif
        }

        private static int getFederFromYCoord( int y ) {
#if JAVA
            int feder = _KEY[0][0];
            int min_diff = Math.abs( _KEY[0][1] - y );
#else
            int feder = _KEY[0, 0];
            int min_diff = Math.Abs( _KEY[0, 1] - y );
#endif
            int index = 0;
#if JAVA
            int len = _KEY.length;
#else
            int len = _KEY.GetUpperBound( 0 ) + 1;
#endif
            for ( int i = 1; i < len; i++ ) {
#if JAVA
                int diff = Math.abs( _KEY[i][1] - y );
#else
                int diff = Math.Abs( _KEY[i, 1] - y );
#endif
                if ( diff < min_diff ) {
                    index = i;
                    min_diff = diff;
#if JAVA
                    feder = _KEY[i][0];
#else
                    feder = _KEY[i, 0];
#endif
                }
            }
            return feder;
        }

        private static int getYCoordFromFeder( int feder ) {
#if JAVA
            int y = _KEY[0][1];
            int min_diff = Math.Abs( _KEY[0][0] - feder );
#else
            int y = _KEY[0, 1];
            int min_diff = Math.Abs( _KEY[0, 0] - feder );
#endif
            int index = 0;
#if JAVA
            int len = _KEY.length;
#else
            int len = _KEY.GetUpperBound( 0 ) + 1;
#endif
            for ( int i = 1; i <= len; i++ ) {
#if JAVA
                int diff = Math.Abs( _KEY[i][0] - feder );
#else
                int diff = Math.Abs( _KEY[i, 0] - feder );
#endif
                if ( diff < min_diff ) {
                    index = i;
                    min_diff = diff;
#if JAVA
                    y = _KEY[i][1];
#else
                    y = _KEY[i, 1];
#endif
                }
            }
            return y;
        }

        private void trackFeder_ValueChanged( Object sender, BEventArgs e ) {
            m_feder = getFederFromYCoord( 151 - (trackFeder.getValue() - 26) );
            txtFeder.setText( (m_feder / 10.0).ToString() );
#if JAVA
            try{
                federChangedEvent.raise( this, new BEventArgs() );
            }catch( Exception ex ){
                System.err.println( "VolumeTracker#trackFeder_ValueChanged; ex=" + ex );
            }
#else
            if ( FederChanged != null ) {
                FederChanged( this, new EventArgs() );
            }
#endif
        }

        private void trackPanpot_ValueChanged( Object sender, BEventArgs e ) {
            txtPanpot.setText( trackPanpot.getValue().ToString() );
#if JAVA
            try{
                panpotChangedEvent.raise( this, new BEventArgs() );
            }catch( Exception ex ){
                System.err.println( "VolumeTracker#trackPanpot_ValueChanged; ex=" + ex );
            }
#else
            if ( PanpotChanged != null ) {
                PanpotChanged( this, new EventArgs() );
            }
#endif
        }

        private void txtFeder_KeyDown( Object sender, BKeyEventArgs e ) {
#if JAVA
            if( (e.getKeyCode() & KeyEvent.VK_ENTER) != KeyEvent.VK_ENTER ){
                return;
            }
#else
            if ( (e.KeyCode & Keys.Enter) != Keys.Enter ) {
                return;
            }
#endif
            try {
                int feder = (int)(PortUtil.parseFloat( txtFeder.getText() ) * 10.0f);
                if ( 55 < feder ) {
                    feder = 55;
                }
                if ( feder < -898 ) {
                    feder = -898;
                }
                setFeder( feder );
                txtFeder.setText( getFeder() / 10.0f + "" );
                txtFeder.requestFocusInWindow();
                txtFeder.selectAll();
            } catch ( Exception ex ) {
            }
        }

        private void txtPanpot_KeyDown( Object sender, BKeyEventArgs e ) {
#if JAVA
            if( (e.getKeyCode() & KeyEvent.VK_ENTER) != KeyEvent.VK_ENTER ){
                return;
            }
#else
            if ( (e.KeyCode & Keys.Enter) != Keys.Enter ) {
                return;
            }
#endif
            try {
                int panpot = PortUtil.parseInt( txtPanpot.Text );
                if ( panpot < -64 ) {
                    panpot = -64;
                }
                if ( 64 < panpot ) {
                    panpot = 64;
                }
                setPanpot( panpot );
                txtPanpot.setText( getPanpot() + "" );
                txtPanpot.requestFocusInWindow();
                txtPanpot.selectAll();
            } catch ( Exception ex ) {
            }
        }

        private void registerEventHandlers() {
#if JAVA
            trackFeder.valueChangedEvent.add( new BEventHandler( this, "trackFeder_ValueChanged" ) );
            trackPanpot.valueChangedEvent.add( new BEventHandler( this, "trackPanpot_ValueChanged" ) );
            txtPanpot.keyDownEvent.add( new BKeyEventHandler( this, "txtPanpot_KeyDown" ) );
            txtFeder.keyDownEvent.add( new BKeyEventHandler( this, "txtFeder_KeyDown" ) );
            //this.Resize += new System.EventHandler( this.VolumeTracker_Resize );
#else
            this.trackFeder.ValueChanged += new System.EventHandler( this.trackFeder_ValueChanged );
            this.trackPanpot.ValueChanged += new System.EventHandler( this.trackPanpot_ValueChanged );
            this.txtPanpot.KeyDown += new System.Windows.Forms.KeyEventHandler( this.txtPanpot_KeyDown );
            this.txtFeder.KeyDown += new System.Windows.Forms.KeyEventHandler( this.txtFeder_KeyDown );
            this.Resize += new System.EventHandler( this.VolumeTracker_Resize );
#endif
        }

        private void setResources() {
        }

#if JAVA
        #region UI Impl for Java
	    private JTextField txtFeder = null;
	    private JSlider trackFeder = null;
	    private JSlider trackPanpot = null;
	    private JTextField txtPanpot = null;
	    private JLabel lblTitle = null;

        /**
	     * This method initializes this
	     * 
	     * @return void
	     */
	    private void initialize() {
		    GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
		    gridBagConstraints11.gridx = 0;
		    gridBagConstraints11.fill = GridBagConstraints.BOTH;
		    gridBagConstraints11.weightx = 1.0D;
		    gridBagConstraints11.gridy = 4;
		    lblTitle = new JLabel();
		    lblTitle.setText("TITLE");
		    lblTitle.setPreferredSize(new Dimension(85, 23));
		    lblTitle.setBackground(Color.white);
		    lblTitle.setHorizontalAlignment(SwingConstants.CENTER);
		    GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
		    gridBagConstraints3.fill = GridBagConstraints.HORIZONTAL;
		    gridBagConstraints3.gridy = 3;
		    gridBagConstraints3.weightx = 1.0;
		    gridBagConstraints3.insets = new Insets(0, 10, 10, 10);
		    gridBagConstraints3.gridx = 0;
		    GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
		    gridBagConstraints2.fill = GridBagConstraints.HORIZONTAL;
		    gridBagConstraints2.gridy = 2;
		    gridBagConstraints2.weightx = 1.0;
		    gridBagConstraints2.weighty = 0.0D;
		    gridBagConstraints2.insets = new Insets(3, 3, 3, 3);
		    gridBagConstraints2.gridx = 0;
		    GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
		    gridBagConstraints1.fill = GridBagConstraints.VERTICAL;
		    gridBagConstraints1.gridy = 1;
		    gridBagConstraints1.weightx = 1.0;
		    gridBagConstraints1.weighty = 1.0D;
		    gridBagConstraints1.insets = new Insets(10, 0, 10, 0);
		    gridBagConstraints1.gridx = 0;
		    GridBagConstraints gridBagConstraints = new GridBagConstraints();
		    gridBagConstraints.fill = GridBagConstraints.HORIZONTAL;
		    gridBagConstraints.gridy = 0;
		    gridBagConstraints.weightx = 1.0;
		    gridBagConstraints.weighty = 0.0D;
		    gridBagConstraints.insets = new Insets(10, 3, 0, 3);
		    gridBagConstraints.gridx = 0;
		    this.setSize(85, 261);
		    this.setLayout(new GridBagLayout());
		    this.setPreferredSize(new Dimension(85, 261));
		    this.setBackground(new Color(180, 180, 180));
		    this.add(getTxtFeder(), gridBagConstraints);
		    this.add(getTrackFeder(), gridBagConstraints1);
		    this.add(getTrackPanpot(), gridBagConstraints2);
		    this.add(getTxtPanpot(), gridBagConstraints3);
		    this.add(lblTitle, gridBagConstraints11);
	    }

	    /**
	     * This method initializes txtFeder	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtFeder() {
		    if (txtFeder == null) {
			    txtFeder = new JTextField();
			    txtFeder.setPreferredSize(new Dimension(79, 19));
			    txtFeder.setHorizontalAlignment(JTextField.CENTER);
			    txtFeder.setText("0");
		    }
		    return txtFeder;
	    }

	    /**
	     * This method initializes trackFeder	
	     * 	
	     * @return javax.swing.JSlider	
	     */
	    private JSlider getTrackFeder() {
		    if (trackFeder == null) {
			    trackFeder = new JSlider();
			    trackFeder.setOrientation(JSlider.VERTICAL);
			    trackFeder.setMinimum(26);
			    trackFeder.setPreferredSize(new Dimension(45, 144));
			    trackFeder.setValue(100);
			    trackFeder.setBackground(new Color(180, 180, 180));
			    trackFeder.setMajorTickSpacing(10);
			    trackFeder.setMaximum(151);
		    }
		    return trackFeder;
	    }

	    /**
	     * This method initializes trackPanpot	
	     * 	
	     * @return javax.swing.JSlider	
	     */
	    private JSlider getTrackPanpot() {
		    if (trackPanpot == null) {
			    trackPanpot = new JSlider();
			    trackPanpot.setMaximum(64);
			    trackPanpot.setValue(0);
			    trackPanpot.setBackground(new Color(180, 180, 180));
			    trackPanpot.setMajorTickSpacing(1);
			    trackPanpot.setMinimum(-64);
		    }
		    return trackPanpot;
	    }

	    /**
	     * This method initializes txtPanpot	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtPanpot() {
		    if (txtPanpot == null) {
			    txtPanpot = new JTextField();
			    txtPanpot.setHorizontalAlignment(JTextField.CENTER);
			    txtPanpot.setText("0");
		    }
		    return txtPanpot;
	    }

        #endregion
#else
        #region UI Impl for C#
        /// <summary> 
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( boolean disposing ) {
            if ( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.trackFeder = new bocoree.windows.forms.BSlider();
            this.trackPanpot = new bocoree.windows.forms.BSlider();
            this.txtPanpot = new bocoree.windows.forms.BTextBox();
            this.lblTitle = new bocoree.windows.forms.BLabel();
            this.txtFeder = new bocoree.windows.forms.BTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackFeder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackPanpot)).BeginInit();
            this.SuspendLayout();
            // 
            // trackFeder
            // 
            this.trackFeder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackFeder.AutoSize = false;
            this.trackFeder.Location = new System.Drawing.Point( 21, 35 );
            this.trackFeder.Maximum = 151;
            this.trackFeder.Minimum = 26;
            this.trackFeder.Name = "trackFeder";
            this.trackFeder.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackFeder.Size = new System.Drawing.Size( 45, 144 );
            this.trackFeder.TabIndex = 0;
            this.trackFeder.TickFrequency = 10;
            this.trackFeder.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackFeder.Value = 100;
            // 
            // trackPanpot
            // 
            this.trackPanpot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackPanpot.AutoSize = false;
            this.trackPanpot.Location = new System.Drawing.Point( 3, 185 );
            this.trackPanpot.Margin = new System.Windows.Forms.Padding( 3, 3, 3, 0 );
            this.trackPanpot.Maximum = 64;
            this.trackPanpot.Minimum = -64;
            this.trackPanpot.Name = "trackPanpot";
            this.trackPanpot.Size = new System.Drawing.Size( 79, 21 );
            this.trackPanpot.TabIndex = 2;
            this.trackPanpot.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // txtPanpot
            // 
            this.txtPanpot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPanpot.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))) );
            this.txtPanpot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPanpot.Location = new System.Drawing.Point( 10, 206 );
            this.txtPanpot.Margin = new System.Windows.Forms.Padding( 10, 0, 10, 0 );
            this.txtPanpot.Name = "txtPanpot";
            this.txtPanpot.Size = new System.Drawing.Size( 65, 19 );
            this.txtPanpot.TabIndex = 3;
            this.txtPanpot.Text = "0";
            this.txtPanpot.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.White;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTitle.Location = new System.Drawing.Point( 0, 238 );
            this.lblTitle.Margin = new System.Windows.Forms.Padding( 0 );
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size( 85, 23 );
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "TITLE";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtFeder
            // 
            this.txtFeder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFeder.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))) );
            this.txtFeder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFeder.Location = new System.Drawing.Point( 3, 10 );
            this.txtFeder.Name = "txtFeder";
            this.txtFeder.Size = new System.Drawing.Size( 79, 19 );
            this.txtFeder.TabIndex = 5;
            this.txtFeder.Text = "0";
            this.txtFeder.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // VolumeTracker
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))) );
            this.Controls.Add( this.txtFeder );
            this.Controls.Add( this.lblTitle );
            this.Controls.Add( this.txtPanpot );
            this.Controls.Add( this.trackPanpot );
            this.Controls.Add( this.trackFeder );
            this.DoubleBuffered = true;
            this.Name = "VolumeTracker";
            this.Size = new System.Drawing.Size( 85, 261 );
            ((System.ComponentModel.ISupportInitialize)(this.trackFeder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackPanpot)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private BSlider trackFeder;
        private BSlider trackPanpot;
        private BTextBox txtPanpot;
        private BLabel lblTitle;
        private BTextBox txtFeder;
        #endregion
#endif
    }

}
