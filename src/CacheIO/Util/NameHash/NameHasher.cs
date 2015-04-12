namespace CacheIO.Util.NameHash
{
	public class NameHasher
	{
		public static int getNameHash(string name)
		{
			int hash = 0;
			name = name.ToLower();

			for (int i = 0; i < name.Length; i++)
			{
				hash = method1258(name.ToCharArray()[i]) + ((hash << 5) - hash);
			}

			return hash;
		}

		private static byte method1258(char c)
		{
			int charByte;
			if (((c > 0) && (c < '')) || ((c >= ' ') && (c <= 'ÿ')))
			{
				charByte = c;
			}
			else
			{
				if (c != '€')
				{
					if (c != '‚')
					{
						if (c != 'ƒ')
						{
							if (c == '„')
							{
								charByte = -124;
							}
							else
							{
								if (c != '…')
								{
									if (c != '†')
									{
										if (c == '‡')
										{
											charByte = -121;
										}
										else
										{
											if (c == 'ˆ')
											{
												charByte = -120;
											}
											else
											{
												if (c == '‰')
												{
													charByte = -119;
												}
												else
												{
													if (c == 'Š')
													{
														charByte = -118;
													}
													else
													{
														if (c == '‹')
														{
															charByte = -117;
														}
														else
														{
															if (c == 'Œ')
															{
																charByte = -116;
															}
															else
															{
																if (c != 'Ž')
																{
																	if (c == '‘')
																	{
																		charByte = -111;
																	}
																	else
																	{
																		if (c != '’')
																		{
																			if (c != '“')
																			{
																				if (c == '”')
																				{
																					charByte = -108;
																				}
																				else
																				{
																					if (c != '•')
																					{
																						if (c == '–')
																						{
																							charByte = -106;
																						}
																						else
																						{
																							if (c == '—')
																							{
																								charByte = -105;
																							}
																							else
																							{
																								if (c == '˜')
																								{
																									charByte = -104;
																								}
																								else
																								{
																									if (c == '™')
																									{
																										charByte = -103;
																									}
																									else
																									{
																										if (c != 'š')
																										{
																											if (c == '›')
																											{
																												charByte = -101;
																											}
																											else
																											{
																												if (c != 'œ')
																												{
																													if (c == 'ž')
																													{
																														charByte = -98;
																													}
																													else
																													{
																														if (c != 'Ÿ')
																															charByte = 63;
																														else
																															charByte = -97;
																													}
																												}
																												else
																												{
																													charByte = -100;
																												}
																											}
																										}
																										else { charByte = -102; }
																									}
																								}
																							}
																						}
																					}
																					else { charByte = -107; }
																				}
																			}
																			else
																			{
																				charByte = -109;
																			}
																		}
																		else
																		{
																			charByte = -110;
																		}
																	}
																}
																else { charByte = -114; }
															}
														}
													}
												}
											}
										}
									}
									else { charByte = -122; }
								}
								else
								{
									charByte = -123;
								}
							}
						}
						else { charByte = -125; }
					}
					else
					{
						charByte = -126;
					}
				}
				else
				{
					charByte = -128;
				}
			}
			return (byte)charByte;
		}
	}
}
