﻿/*
 * IconParameter.cs
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;

#else
using System;
using org.kbinani.java.io;

namespace org.kbinani.vsq{
#endif

    [Serializable]
    public class IconParameter {
        public enum ArticulationType {
            Vibrato,
            Crescendo,
            Dynaff,
            NoteAttack,
            NoteTransition,
        }

        protected ArticulationType articulation;
        protected String button = "";
        protected String caption = "";

        protected int length;
        protected int startDepth;
        protected int endDepth;
        protected int startRate;
        protected int endRate;
        protected int startDyn;
        protected int endDyn;
        protected int duration;
        protected int depth;
        protected VibratoBPList dynBP;
        protected VibratoBPList depthBP;
        protected VibratoBPList rateBP;

        protected String buttonImageFullPath = "";

        protected IconParameter() {
        }

        protected IconParameter( String file ) {
            if ( file == null ) {
                return;
            }
            if ( file.Equals( "" ) ) {
                return;
            }
            BufferedReader sr = null;
            try {
                sr = new BufferedReader( new InputStreamReader( new FileInputStream( file ), "Shift_JIS" ) );
                String line = "";
                String strDynBPNum = "";
                String strDynBPX = "";
                String strDynBPY = "";
                String strDepthBPNum = "";
                String strDepthBPX = "";
                String strDepthBPY = "";
                String strRateBPNum = "";
                String strRateBPX = "";
                String strRateBPY = "";
                while ( (line = sr.readLine()) != null ) {
                    // コメントを除去する
                    int indx_colon = line.IndexOf( ';' );
                    if ( indx_colon >= 0 ) {
                        line = line.Substring( 0, indx_colon );
                    }
                    // セクション名の指定行
                    if ( line.StartsWith( "[" ) ) {
                        continue;
                    }
                    // イコールが含まれているかどうか
                    String[] spl = PortUtil.splitString( line, new char[]{ '=' }, 2 );
                    if ( spl.Length != 2 ) {
                        continue;
                    }
                    String name = spl[0].Trim( new char[]{ ' ', '\t' } );
                    String value = spl[1].Trim( new char[]{ ' ', '\t' } );
                    if ( name.Equals( "Articulation" ) ) {
                        if ( value.Equals( "Vibrato" ) ) {
                            articulation = ArticulationType.Vibrato;
                        } else if ( value.Equals( "Crescendo" ) ) {
                            articulation = ArticulationType.Crescendo;
                        } else if ( value.Equals( "Dynaff" ) ) {
                            articulation = ArticulationType.Dynaff;
                        } else if ( value.Equals( "NoteAttack" ) ) {
                            articulation = ArticulationType.NoteAttack;
                        } else if ( value.Equals( "NoteTransition" ) ) {
                            articulation = ArticulationType.NoteTransition;
                        }
                    } else if ( name.Equals( "Button" ) ) {
                        button = value;
                    } else if ( name.Equals( "Caption" ) ) {
                        caption = value;
                    } else if ( name.Equals( "Length" ) ) {
                        try {
                            length = PortUtil.parseInt( value );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( name.Equals( "StartDepth" ) ) {
                        try {
                            startDepth = PortUtil.parseInt( value );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( name.Equals( "EndDepth" ) ) {
                        try {
                            endDepth = PortUtil.parseInt( value );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( name.Equals( "StartRate" ) ) {
                        try {
                            startRate = PortUtil.parseInt( value );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( name.Equals( "EndRate" ) ) {
                        try {
                            endRate = PortUtil.parseInt( value );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( name.Equals( "StartDyn" ) ) {
                        try {
                            startDyn = PortUtil.parseInt( value );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( name.Equals( "EndDyn" ) ) {
                        try {
                            endDyn = PortUtil.parseInt( value );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( name.Equals( "Duration" ) ) {
                        try {
                            duration = PortUtil.parseInt( value );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( name.Equals( "Depth" ) ) {
                        try {
                            depth = PortUtil.parseInt( value );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        }
                    } else if ( name.Equals( "DynBPNum" ) ) {
                        strDynBPNum = value;
                    } else if ( name.Equals( "DynBPX" ) ) {
                        strDynBPX = value;
                    } else if ( name.Equals( "DynBPY" ) ) {
                        strDynBPY = value;
                    } else if ( name.Equals( "DepthBPNum" ) ) {
                        strDepthBPNum = value;
                    } else if ( name.Equals( "DepthBPX" ) ) {
                        strDepthBPX = value;
                    } else if ( name.Equals( "DepthBPY" ) ) {
                        strDepthBPY = value;
                    } else if ( name.Equals( "RateBPNum" ) ) {
                        strRateBPNum = value;
                    } else if ( name.Equals( "RateBPX" ) ) {
                        strRateBPX = value;
                    } else if ( name.Equals( "RateBPY" ) ) {
                        strRateBPY = value;
                    }
                }
                if ( !strDynBPNum.Equals( "" ) ) {
                    int num = 0;
                    try {
                        num = PortUtil.parseInt( strDynBPNum );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                        num = 0;
                    }
                    String[] strBPX = PortUtil.splitString( strDynBPX, ',' );
                    String[] strBPY = PortUtil.splitString( strDynBPY, ',' );
                    int actNum = Math.Min( num, Math.Min( strBPX.Length, strBPY.Length ) );
                    if ( actNum > 0 ) {
                        float[] x = new float[actNum];
                        int[] y = new int[actNum];
                        for ( int i = 0; i < actNum; i++ ) {
                            try {
                                x[i] = PortUtil.parseFloat( strBPX[i] );
                                y[i] = PortUtil.parseInt( strBPY[i] );
                            } catch ( Exception ex ) {
                                PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                            }
                        }
                        dynBP = new VibratoBPList( x, y );
                    }
                }
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
            }
        }

        public String getButton() {
            return button;
        }
        
        public String getButtonImageFullPath() {
            return buttonImageFullPath;
        }

        public void setButtonImageFullPath( String value ) {
            buttonImageFullPath = value;
        }
    }

#if !JAVA
}
#endif